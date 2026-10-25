import { useEffect } from 'react'
import { ErrorBoundary } from '../components/shared/ErrorBoundary'
import { useCourse } from '../features/courses/queries'
import { CourseQueryFailure, CourseRefreshFailure } from '../features/courses/CourseQueryState'
import { MarketingLayout } from '../components/layout/MarketingLayout'
import { CourseDetailsHero } from '../features/course-details/CourseDetailsHero'
import { CourseDetailsContent } from '../features/course-details/CourseDetailsContent'
import { CourseDetailsSkeleton } from '../features/course-details/CourseDetailsSkeleton'
import { EnrollmentBar } from '../features/course-details/EnrollmentBar'
import { ContactCta } from '../features/course-details/ContactCta'
import { Testimonials } from '../components/shared/Testimonials'

export function CourseDetailsPage({ courseId }: { courseId: number }) {
  const courseQuery = useCourse(courseId)

  useEffect(() => {
    if (courseQuery.data) {
      document.title = `${courseQuery.data.fullName} — SESI Indústria`
    } else if (courseQuery.isError) {
      document.title = courseQuery.error instanceof Error && 'status' in courseQuery.error && courseQuery.error.status === 404
        ? 'Curso não encontrado — SESI Indústria'
        : 'Não foi possível carregar o curso — SESI Indústria'
    }
  }, [courseQuery.data?.fullName, courseQuery.error, courseQuery.isError])

  if (!courseQuery.data) {
    if (courseQuery.isError) {
      return <CourseQueryFailure error={courseQuery.error} onRetry={() => void courseQuery.refetch()} />
    }
    return <CourseDetailsSkeleton />
  }

  const course = courseQuery.data
  return (
    <MarketingLayout variant="details">
      <ErrorBoundary
        key={`details-${course.id}`}
        contextKey={`details-${course.id}`}
        title="Não foi possível mostrar os detalhes deste curso"
        description="O conteúdo da página encontrou um problema inesperado. Tente recarregar os dados ou volte para o início."
        returnHref="/"
        returnLabel="Voltar para o início"
        onRetry={() => courseQuery.refetch()}
      >
        <main tabIndex={-1}>
          {courseQuery.isError && <CourseRefreshFailure error={courseQuery.error} onRetry={() => void courseQuery.refetch()} />}
          <CourseDetailsHero course={course} />
          <CourseDetailsContent course={course} />
          <Testimonials variant="details" />
          <ContactCta />
          <EnrollmentBar course={course} />
        </main>
      </ErrorBoundary>
    </MarketingLayout>
  )
}
