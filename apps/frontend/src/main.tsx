import '@mantine/core/styles.css'
import './styles/fonts.css'
import './styles/global.css'

import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { App } from './app/App'
import { ErrorBoundary } from './components/shared/ErrorBoundary'

const rootElement = document.getElementById('root')

if (!rootElement) {
  throw new Error('Root element #root was not found')
}

createRoot(rootElement).render(
  <StrictMode>
    <ErrorBoundary
      contextKey="application"
      title="A aplicação encontrou um problema"
      description="Não foi possível mostrar esta tela. Volte para o início para continuar."
      returnHref="/"
      returnLabel="Voltar para o início"
    >
      <App />
    </ErrorBoundary>
  </StrictMode>,
)
