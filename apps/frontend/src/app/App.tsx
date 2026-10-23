import { MantineProvider } from '@mantine/core'
import { BrowserRouter } from 'react-router-dom'
import { theme } from './theme'
import { AppRoutes } from './routes'

export function App() {
  return (
    <MantineProvider theme={theme} forceColorScheme="light">
      <BrowserRouter>
        <AppRoutes />
      </BrowserRouter>
    </MantineProvider>
  )
}
