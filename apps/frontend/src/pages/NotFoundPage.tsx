import { Link } from 'react-router-dom'

export function NotFoundPage() {
  return (
    <main tabIndex={-1}>
      <h1>Página não encontrada</h1>
      <p>O endereço não corresponde a uma página disponível.</p>
      <Link to="/">Voltar para a página inicial</Link>
    </main>
  )
}
