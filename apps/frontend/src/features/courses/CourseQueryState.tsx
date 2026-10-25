import { Button } from '@mantine/core'
import { Link } from 'react-router-dom'
import { ApiError } from '../../lib/api-client'

export function CourseLoadingState({ label, backHref, backLabel }: {
  label: string
  backHref?: string
  backLabel?: string
}) {
  return (
    <main tabIndex={-1} aria-busy="true">
      <p role="status">{label}</p>
      {backHref && <Link to={backHref}>{backLabel ?? 'Voltar'}</Link>}
    </main>
  )
}

export function CourseQueryFailure({ error, onRetry, backHref, backLabel }: {
  error: Error
  onRetry: () => void
  backHref?: string
  backLabel?: string
}) {
  if (error instanceof ApiError && error.status === 404) {
    return (
      <main tabIndex={-1}>
        <h1>Curso não encontrado</h1>
        <p>Este curso não está disponível no catálogo consultado.</p>
        <Link to="/">Voltar para o início</Link>
      </main>
    )
  }

  return (
    <main tabIndex={-1}>
      <h1>Não foi possível carregar o curso</h1>
      <p role="alert">{courseErrorMessage(error)} Tente novamente ou volte para o início.</p>
      <Button type="button" onClick={onRetry}>Tentar novamente</Button>
      {' '}
      <Link to={backHref ?? '/'}>{backLabel ?? 'Voltar para o início'}</Link>
    </main>
  )
}

export function CourseRefreshFailure({ error, onRetry }: { error: Error; onRetry: () => void }) {
  return (
    <p role="status">
      {courseErrorMessage(error)} Os dados carregados continuam disponíveis.{' '}
      <Button type="button" variant="subtle" onClick={onRetry}>Tentar novamente</Button>
    </p>
  )
}

export function courseErrorMessage(error: Error): string {
  if (!(error instanceof ApiError)) return 'Não foi possível carregar o catálogo de cursos.'

  switch (error.kind) {
    case 'configuration':
      return 'A configuração do endereço da API não é válida.'
    case 'invalid-response':
      return 'O catálogo retornou dados incompatíveis com esta tela.'
    case 'timeout':
      return 'O catálogo demorou mais que o esperado para responder.'
    case 'network':
      return 'Não foi possível conectar ao catálogo de cursos.'
    case 'http':
      return error.status !== undefined && error.status >= 500
        ? 'O catálogo está temporariamente indisponível.'
        : 'A consulta ao catálogo não pôde ser concluída.'
    case 'cancelled':
      return 'A consulta ao catálogo foi cancelada.'
  }
}
