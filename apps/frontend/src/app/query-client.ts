import { QueryClient } from '@tanstack/react-query'
import { shouldRetryApiError } from '../lib/api-client'

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 60_000,
      gcTime: 5 * 60_000,
      retry: shouldRetryApiError,
    },
  },
})
