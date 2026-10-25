import axios, { AxiosError } from 'axios'

const DEFAULT_API_BASE_URL = 'http://localhost:5080'

export type ApiFailureKind = 'configuration' | 'invalid-response' | 'cancelled' | 'network' | 'timeout' | 'http'

export class ApiError extends Error {
  constructor(
    readonly kind: ApiFailureKind,
    message: string,
    readonly status?: number,
  ) {
    super(message)
    this.name = 'ApiError'
  }
}

function resolveApiBaseUrl(value: string | undefined): { baseUrl: string; error: ApiError | null } {
  if (value === undefined || value === '') {
    return { baseUrl: DEFAULT_API_BASE_URL, error: null }
  }

  if (value !== value.trim()) {
    return {
      baseUrl: DEFAULT_API_BASE_URL,
      error: new ApiError('configuration', 'VITE_API_BASE_URL must be an HTTP or HTTPS origin.'),
    }
  }

  try {
    const url = new URL(value)
    if (
      (url.protocol !== 'http:' && url.protocol !== 'https:') ||
      url.username !== '' ||
      url.password !== '' ||
      url.pathname !== '/' ||
      url.search !== '' ||
      url.hash !== ''
    ) {
      throw new Error('Invalid API origin')
    }

    return { baseUrl: url.origin, error: null }
  } catch {
    return {
      baseUrl: DEFAULT_API_BASE_URL,
      error: new ApiError(
        'configuration',
        'VITE_API_BASE_URL must be an absolute HTTP or HTTPS origin without credentials, path, query, or fragment.',
      ),
    }
  }
}

const apiBaseUrl = resolveApiBaseUrl(import.meta.env.VITE_API_BASE_URL)

export const apiClient = axios.create({
  baseURL: apiBaseUrl.baseUrl,
  timeout: 10_000,
  withCredentials: false,
  headers: { Accept: 'application/json' },
})

export function assertApiConfiguration(): void {
  if (apiBaseUrl.error) {
    throw apiBaseUrl.error
  }
}

export function toApiError(error: unknown): ApiError {
  if (error instanceof ApiError) {
    return error
  }

  if (axios.isCancel(error) || (axios.isAxiosError(error) && error.code === AxiosError.ERR_CANCELED)) {
    return new ApiError('cancelled', 'The course API request was cancelled.')
  }

  if (axios.isAxiosError(error)) {
    if (error.code === AxiosError.ETIMEDOUT || error.code === AxiosError.ECONNABORTED) {
      return new ApiError('timeout', 'The course API request timed out.')
    }
    if (error.response) {
      return new ApiError('http', `The course API returned HTTP ${error.response.status}.`, error.response.status)
    }
  }

  return new ApiError('network', 'The course API could not be reached.')
}

export function shouldRetryApiError(failureCount: number, error: unknown): boolean {
  if (failureCount >= 1 || !(error instanceof ApiError)) {
    return false
  }

  return error.kind === 'network' ||
    error.kind === 'timeout' ||
    (error.kind === 'http' && error.status !== undefined && error.status >= 500)
}
