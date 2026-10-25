import { useEffect } from 'react'
import { useLocation } from 'react-router-dom'
import { parseCourseId } from './course-paths'
import { routePaths } from './routes'

function getDocumentTitle(pathname: string) {
  if (pathname === routePaths.home) return 'Início — SESI Indústria'
  if (pathname.startsWith('/cursos/') && parseCourseId(pathname.split('/').at(-1)) !== null) {
    return 'Detalhes do curso — SESI Indústria'
  }
  if (pathname.startsWith('/ava/') && parseCourseId(pathname.split('/').at(-1)) !== null) {
    return 'Prévia de aprendizagem — SESI Indústria'
  }
  return 'Página não encontrada — SESI Indústria'
}

export function RouteEffects() {
  const { pathname, hash } = useLocation()

  useEffect(() => {
    document.title = getDocumentTitle(pathname)
    window.history.scrollRestoration = 'manual'

    const frame = window.requestAnimationFrame(() => {
      if (hash) {
        const targetId = decodeURIComponent(hash.slice(1))
        const target = document.getElementById(targetId)
        if (target instanceof HTMLElement) {
          target.scrollIntoView()
          target.focus({ preventScroll: true })
        }
        return
      }

      window.scrollTo({ top: 0, left: 0, behavior: 'instant' })

      const main = document.querySelector('main')
      if (main instanceof HTMLElement) main.focus({ preventScroll: true })
    })

    return () => window.cancelAnimationFrame(frame)
  }, [pathname, hash])

  return null
}
