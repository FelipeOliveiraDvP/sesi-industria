import { MantineProvider } from '@mantine/core'
import { QueryClientProvider } from '@tanstack/react-query'
import { BrowserRouter } from 'react-router-dom'
import { theme } from './theme'
import { queryClient } from './query-client'
import { AppRoutes } from './routes'

export function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <MantineProvider theme={theme} forceColorScheme="light">
        <BrowserRouter>
          <AppRoutes />
        </BrowserRouter>
      </MantineProvider>
    </QueryClientProvider>
  )
}
