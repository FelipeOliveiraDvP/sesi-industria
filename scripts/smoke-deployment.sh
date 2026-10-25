#!/usr/bin/env bash

set -Eeuo pipefail

script_dir="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
repo_root="$(cd -- "$script_dir/.." && pwd)"
run_id="$(date -u +%Y%m%d%H%M%S)-$$"
network="sesi-deploy-smoke-${run_id}"
database_container="sesi-deploy-smoke-postgres-${run_id}"
api_default_container="sesi-deploy-smoke-api-default-${run_id}"
api_alternate_container="sesi-deploy-smoke-api-alternate-${run_id}"
frontend_default_container="sesi-deploy-smoke-frontend-default-${run_id}"
frontend_alternate_container="sesi-deploy-smoke-frontend-alternate-${run_id}"
api_image="sesi/deploy-smoke-api:${run_id}"
frontend_image="sesi/deploy-smoke-frontend:${run_id}"
api_origin="https://api.example"
frontend_origin="https://frontend.example"
database_connection='Host=postgres;Port=5432;Database=catalog_smoke;Username=catalog_smoke;Password=catalog_smoke_password'
created_containers=()
created_images=()
http_body=""
http_status=""

fail() {
  printf 'Smoke test failed: %s\n' "$*" >&2
  exit 1
}

cleanup() {
  local exit_code=$?
  trap - EXIT

  local container
  for container in "${created_containers[@]}"; do
    docker rm --force --volumes "$container" >/dev/null 2>&1 || true
  done

  docker network rm "$network" >/dev/null 2>&1 || true

  local image
  for image in "${created_images[@]}"; do
    docker image rm --force "$image" >/dev/null 2>&1 || true
  done

  exit "$exit_code"
}
trap cleanup EXIT

for command in docker curl python3 grep sed date seq; do
  command -v "$command" >/dev/null 2>&1 || fail "required command not found: $command"
done
docker info >/dev/null 2>&1 || fail "Docker daemon is unavailable"

printf 'Building the .NET API image...\n'
created_images+=("$api_image")
docker build \
  --file "$repo_root/apps/backend/Dockerfile" \
  --tag "$api_image" \
  "$repo_root"

printf 'Building the Nginx frontend image...\n'
created_images+=("$frontend_image")
env -u VITE_API_BASE_URL docker build \
  --file "$repo_root/apps/frontend/Dockerfile" \
  --tag "$frontend_image" \
  --build-arg "VITE_API_BASE_URL=$api_origin" \
  "$repo_root"

assert_frontend_build_rejected() {
  local label="$1"
  shift
  local output

  if output="$(env -u VITE_API_BASE_URL docker build \
    --quiet \
    --target build \
    --file "$repo_root/apps/frontend/Dockerfile" \
    "$@" \
    "$repo_root" 2>&1)"; then
    fail "frontend build accepted invalid VITE_API_BASE_URL: $label"
  fi

  if ! grep -F 'VITE_API_BASE_URL must be an absolute HTTP or HTTPS origin' <<<"$output" >/dev/null; then
    printf '%s\n' "$output" >&2
    fail "frontend build failed for an unexpected reason while testing: $label"
  fi

  printf 'Rejected invalid frontend API origin: %s\n' "$label"
}

assert_frontend_build_rejected "missing"
assert_frontend_build_rejected "relative" --build-arg VITE_API_BASE_URL=api.example
assert_frontend_build_rejected "credentials" --build-arg 'VITE_API_BASE_URL=https://user:secret@api.example'
assert_frontend_build_rejected "subpath" --build-arg VITE_API_BASE_URL=https://api.example/v1
assert_frontend_build_rejected "query" --build-arg 'VITE_API_BASE_URL=https://api.example?mode=demo'
assert_frontend_build_rejected "fragment" --build-arg 'VITE_API_BASE_URL=https://api.example#demo'

docker network create "$network" >/dev/null
created_containers+=("$database_container")
docker run --detach \
  --name "$database_container" \
  --network "$network" \
  --network-alias postgres \
  --shm-size 256m \
  --tmpfs /var/lib/postgresql/data:rw,size=512m \
  --env POSTGRES_DB=catalog_smoke \
  --env POSTGRES_USER=catalog_smoke \
  --env POSTGRES_PASSWORD=catalog_smoke_password \
  postgres:17 >/dev/null

database_ready=0
for attempt in $(seq 1 60); do
  if docker exec "$database_container" pg_isready -U catalog_smoke -d catalog_smoke >/dev/null 2>&1; then
    database_ready=1
    break
  fi
  sleep 1
done
if [[ "$database_ready" -ne 1 ]]; then
  docker logs "$database_container" >&2
  fail "disposable PostgreSQL did not become ready in 60 seconds"
fi

wait_for_http() {
  local container="$1"
  local url="$2"
  local path="$3"
  local expected_status="$4"
  local attempts="$5"
  local response
  local status

  for attempt in $(seq 1 "$attempts"); do
    response="$(curl --silent --show-error --connect-timeout 2 --max-time 5 \
      --write-out $'\n%{http_code}' "$url$path" 2>/dev/null || true)"
    status="${response##*$'\n'}"
    if [[ "$status" == "$expected_status" ]]; then
      http_status="$status"
      http_body="${response%$'\n'*}"
      return 0
    fi
    sleep 1
  done

  docker logs "$container" >&2 || true
  fail "$url$path did not return HTTP $expected_status within ${attempts}s (last status: ${status:-none})"
}

start_api() {
  local name="$1"
  local port="$2"
  local container_port="$3"
  local host_port

  docker run --detach \
    --name "$name" \
    --network "$network" \
    --publish "127.0.0.1::${container_port}" \
    --env "PORT=$port" \
    --env "ConnectionStrings__DefaultConnection=$database_connection" \
    --env "FRONTEND_URL=$frontend_origin" \
    --env Database__InitializeOnStartup=true \
    --env ASPNETCORE_ENVIRONMENT=Production \
    "$api_image" >/dev/null

  host_port="$(docker port "$name" "${container_port}/tcp" | sed 's/.*://')"
  printf 'http://127.0.0.1:%s\n' "$host_port"
}

printf 'Starting API on its default port and initializing the disposable database...\n'
created_containers+=("$api_default_container")
api_default_url="$(start_api "$api_default_container" 8080 8080)"
wait_for_http "$api_default_container" "$api_default_url" /api/courses 200 90
api_summary_before="$http_body"
printf '%s' "$api_summary_before" | python3 -c '
import json, sys
payload = json.load(sys.stdin)
assert payload["totalCount"] == 6
assert len(payload["items"]) == 6
'

wait_for_http "$api_default_container" "$api_default_url" /api/courses/1 200 10
api_detail="$http_body"
printf '%s' "$api_detail" | python3 -c '
import json, sys
detail = json.load(sys.stdin)
assert detail["id"] == 1
assert detail["modules"]
presentation = detail["presentation"]
assert presentation["detailsImageUrl"].startswith("https://frontend.example/assets/")
assert presentation["learningPreview"]["bannerImageUrl"].startswith("https://frontend.example/assets/")
'

cors_headers="$(curl --silent --show-error --dump-header - --output /dev/null \
  --header 'Origin: https://smoke-check.example' "$api_default_url/api/courses")"
grep -Fi 'Access-Control-Allow-Origin: *' <<<"$cors_headers" >/dev/null ||
  fail "API did not return the expected public read-only CORS header"

swagger_status="$(curl --silent --output /dev/null --write-out '%{http_code}' "$api_default_url/swagger")"
[[ "$swagger_status" == 404 ]] || fail "Swagger should be unavailable in Production, got HTTP $swagger_status"
printf 'API default port: six courses, course detail, image URLs, CORS and Production Swagger policy verified.\n'

docker rm --force --volumes "$api_default_container" >/dev/null

printf 'Restarting API with an alternate PORT against the same database...\n'
created_containers+=("$api_alternate_container")
api_alternate_url="$(start_api "$api_alternate_container" 8123 8123)"
wait_for_http "$api_alternate_container" "$api_alternate_url" /api/courses 200 90
api_summary_after="$http_body"
python3 - "$api_summary_before" "$api_summary_after" <<'PY'
import json
import sys
before, after = (json.loads(value) for value in sys.argv[1:])
before_items = [(item["id"], item["fullName"]) for item in before["items"]]
after_items = [(item["id"], item["fullName"]) for item in after["items"]]
assert before["totalCount"] == after["totalCount"] == 6
assert before_items == after_items
PY
printf 'API alternate port: restart reused the initialized catalog without duplicates.\n'

start_frontend() {
  local name="$1"
  local port="$2"
  local container_port="$3"
  local host_port

  created_containers+=("$name")
  docker run --detach \
    --name "$name" \
    --publish "127.0.0.1::${container_port}" \
    --env "PORT=$port" \
    --env VITE_API_BASE_URL=https://runtime-override.example \
    "$frontend_image" >/dev/null

  host_port="$(docker port "$name" "${container_port}/tcp" | sed 's/.*://')"
  printf 'http://127.0.0.1:%s\n' "$host_port"
}

printf 'Starting frontend on its default port...\n'
created_containers+=("$frontend_default_container")
frontend_default_url="$(start_frontend "$frontend_default_container" 8080 8080)"
wait_for_http "$frontend_default_container" "$frontend_default_url" / 200 30
frontend_index="$http_body"
for route in \
  /cursos/1 \
  /ava/1 \
  /cursos/matematica-para-a-precisao \
  /ava/matematica-para-a-precisao \
  /rota-desconhecida; do
  wait_for_http "$frontend_default_container" "$frontend_default_url" "$route" 200 10
  [[ "$http_body" == "$frontend_index" ]] || fail "$route did not receive the SPA document"
done

image_result="$(curl --silent --output /dev/null --write-out '%{http_code} %{content_type}' \
  "$frontend_default_url/assets/hero-image.png")"
[[ "$image_result" == '200 image/png' ]] || fail "unexpected PNG asset response: $image_result"

js_path="$(printf '%s' "$frontend_index" | python3 -c '
import re, sys
match = re.search(r"src=\"([^\"]+\.js)\"", sys.stdin.read())
assert match, "bundled JS entry not found in index.html"
print(match.group(1))
')"
js_result="$(curl --silent --output /dev/null --write-out '%{http_code} %{content_type}' \
  "$frontend_default_url$js_path")"
case "$js_result" in
  '200 application/javascript'|'200 text/javascript') ;;
  *) fail "unexpected JavaScript asset response: $js_result" ;;
esac

root_headers="$(curl --silent --head "$frontend_default_url/")"
route_headers="$(curl --silent --head "$frontend_default_url/cursos/1")"
asset_headers="$(curl --silent --head "$frontend_default_url/assets/hero-image.png")"
grep -Fi 'Cache-Control: no-cache, no-store, must-revalidate' <<<"$root_headers" >/dev/null ||
  fail "index.html is missing its revalidation cache policy"
grep -Fi 'Cache-Control: no-cache, no-store, must-revalidate' <<<"$route_headers" >/dev/null ||
  fail "SPA fallback HTML is missing its revalidation cache policy"
grep -Fi 'Cache-Control: no-cache' <<<"$asset_headers" >/dev/null ||
  fail "unhashed public image asset is missing its conservative cache policy"

missing_response="$(curl --silent --show-error --write-out $'\n%{http_code}' \
  "$frontend_default_url/assets/smoke-missing.png" || true)"
missing_status="${missing_response##*$'\n'}"
missing_body="${missing_response%$'\n'*}"
[[ "$missing_status" == 404 ]] || fail "missing asset returned HTTP $missing_status instead of 404"
[[ "$missing_body" != "$frontend_index" ]] || fail "missing asset returned the SPA document"

js_body="$(curl --fail --silent --show-error "$frontend_default_url$js_path")"
[[ "$js_body" == *"$api_origin"* ]] || fail "VITE_API_BASE_URL build value is missing from the bundle"
[[ "$js_body" != *"https://runtime-override.example"* ]] ||
  fail "runtime VITE_API_BASE_URL unexpectedly replaced the compiled build value"
printf 'Frontend default port: SPA routes, MIME, 404, cache headers and build-time API origin verified.\n'

printf 'Starting frontend on an alternate PORT...\n'
created_containers+=("$frontend_alternate_container")
frontend_alternate_url="$(start_frontend "$frontend_alternate_container" 8124 8124)"
wait_for_http "$frontend_alternate_container" "$frontend_alternate_url" /ava/1 200 30
[[ "$http_body" == "$frontend_index" ]] || fail "alternate frontend port did not serve the SPA document"
printf 'Frontend alternate port: direct AVA route returned the SPA document.\n'

printf 'All deployment image smoke checks passed.\n'
