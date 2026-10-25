import { Burger, Drawer } from '@mantine/core'
import { useDisclosure } from '@mantine/hooks'
import { useEffect } from 'react'
import { Link } from 'react-router-dom'
import { ErrorBoundary } from '../../components/shared/ErrorBoundary'
import { courseDetailsPath } from '../../app/course-paths'
import { useCourse } from '../courses/queries'
import { CourseQueryFailure, CourseRefreshFailure } from '../courses/CourseQueryState'
import { LearningArticle } from './LearningArticle'
import { LearningEnvironmentSkeleton } from './LearningEnvironmentSkeleton'
import { LearningSidebar } from './LearningSidebar'
import classes from './LearningEnvironmentPage.module.css'

export function LearningEnvironmentPage({ courseId }: { courseId: number }) {
  const [drawerOpened, drawerHandlers] = useDisclosure(false)
  const courseQuery = useCourse(courseId)

  useEffect(() => {
    if (courseQuery.data) {
      document.title = `Boas práticas — ${courseQuery.data.fullName} | SESI Indústria`
    } else if (courseQuery.isError) {
      document.title = courseQuery.error instanceof Error && 'status' in courseQuery.error && courseQuery.error.status === 404
        ? 'Curso não encontrado — SESI Indústria'
        : 'Não foi possível carregar o curso — SESI Indústria'
    }
  }, [courseQuery.data?.fullName, courseQuery.error, courseQuery.isError])

  if (!courseQuery.data) {
    if (courseQuery.isError) {
      return <CourseQueryFailure
        error={courseQuery.error}
        onRetry={() => void courseQuery.refetch()}
        backHref={courseDetailsPath(courseId)}
        backLabel="Voltar aos detalhes do curso"
      />
    }
    return <LearningEnvironmentSkeleton courseId={courseId} />
  }

  const course = courseQuery.data

  return (
    <ErrorBoundary
      key={`ava-${course.id}`}
      contextKey={`ava-${course.id}`}
      title="Não foi possível mostrar a prévia do curso"
      description="O conteúdo da página encontrou um problema inesperado. Tente recarregar os dados ou volte aos detalhes."
      returnHref={courseDetailsPath(course.id)}
      returnLabel="Voltar aos detalhes do curso"
      onRetry={() => courseQuery.refetch()}
    >
      <div className={classes.layout}>
        <div className={classes.desktopSidebar}>
          <LearningSidebar course={course} />
        </div>

        <main className={classes.main} aria-describedby="ava-demo-note" tabIndex={-1}>
          {courseQuery.isError && <CourseRefreshFailure error={courseQuery.error} onRetry={() => void courseQuery.refetch()} />}
          <header className={classes.mobileToolbar}>
            <Burger
              opened={drawerOpened}
              onClick={drawerHandlers.toggle}
              aria-label={drawerOpened ? 'Fechar conteúdo do curso' : 'Abrir conteúdo do curso'}
              aria-expanded={drawerOpened}
              size="sm"
            />
            <span className={classes.toolbarTitle}>Prévia do ambiente</span>
            <Link className={classes.mobileBackLink} to={courseDetailsPath(course.id)} aria-label="Voltar para os detalhes do curso">
              <span aria-hidden="true">←</span>
            </Link>
          </header>

          <LearningArticle course={course} />
        </main>

        <Drawer
          className={classes.mobileDrawer}
          opened={drawerOpened}
          onClose={drawerHandlers.close}
          title="Conteúdo do curso"
          position="left"
          size="min(86vw, 22rem)"
        >
          <div className={classes.drawerSidebar}>
            <LearningSidebar course={course} onReturn={drawerHandlers.close} />
          </div>
        </Drawer>
      </div>
    </ErrorBoundary>
  )
}
