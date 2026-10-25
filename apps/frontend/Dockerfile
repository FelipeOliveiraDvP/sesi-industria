FROM node:24-alpine AS build
WORKDIR /workspace

RUN npm install --global pnpm@11.9.0

COPY package.json pnpm-lock.yaml pnpm-workspace.yaml ./
COPY apps/frontend/package.json ./apps/frontend/package.json
RUN pnpm install --frozen-lockfile

COPY apps/frontend/ ./apps/frontend/
ARG VITE_API_BASE_URL
ENV VITE_API_BASE_URL=${VITE_API_BASE_URL}
RUN node -e 'const value = process.env.VITE_API_BASE_URL; try { if (!value || value !== value.trim()) throw new Error(); const url = new URL(value); if (!["http:", "https:"].includes(url.protocol) || url.username || url.password || url.pathname !== "/" || url.search || url.hash) throw new Error(); } catch { console.error("VITE_API_BASE_URL must be an absolute HTTP or HTTPS origin without credentials, path, query, or fragment."); process.exit(1); }'
RUN pnpm --filter @sesi/frontend build

FROM nginx:stable-alpine AS runtime
COPY --from=build /workspace/apps/frontend/dist/ /usr/share/nginx/html/
COPY apps/frontend/nginx/default.conf.template /etc/nginx/templates/default.conf.template

ENV PORT=8080 \
    NGINX_ENVSUBST_FILTER=^PORT$

EXPOSE 8080
CMD ["nginx", "-g", "daemon off;"]
