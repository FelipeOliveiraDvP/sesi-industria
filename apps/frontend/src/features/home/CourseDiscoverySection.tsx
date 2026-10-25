import { Button, Skeleton } from '@mantine/core'
import { useReducedMotion } from '@mantine/hooks'
import { Link } from 'react-router-dom'
import { HorizontalRail } from '../../components/shared/HorizontalRail'
import { assetPaths } from '../../app/assets'
import { courseDetailsPath } from '../../app/course-paths'
import { courseErrorMessage } from '../courses/CourseQueryState'
import { useCourses } from '../courses/queries'
import type { CourseSummary } from '../courses/types'
import classes from './CourseDiscoverySection.module.css'

export function CourseDiscoverySection() {
  const coursesQuery = useCourses()
  const reduceMotion = useReducedMotion()

  return (
    <section id="cursos" tabIndex={-1} className={classes.section} aria-labelledby="courses-heading">
      <div className={classes.container}>
        <HorizontalRail
          label="Cursos em destaque"
          heading={<h2 id="courses-heading" className={classes.heading}>Principais cursos</h2>}
          footer={(
            <div className={classes.allCoursesAction}>
              <Button type="button" className={classes.allCoursesButton} disabled aria-label="Ver todos os cursos (indisponível nesta demonstração)">
                Ver todos os cursos
              </Button>
            </div>
          )}
        >
          <ul className={classes.track} aria-busy={coursesQuery.isPending && !coursesQuery.data}>
            {coursesQuery.isPending && !coursesQuery.data && Array.from({ length: 6 }, (_, index) => (
              <li className={classes.trackItem} key={`course-skeleton-${index}`} aria-hidden="true">
                <CourseCardSkeleton animate={!reduceMotion} />
              </li>
            ))}
            {coursesQuery.data?.items.map((course) => (
              <li className={classes.trackItem} key={course.id}>
                <CourseCardView course={course} />
              </li>
            ))}
            {coursesQuery.data && (
              <li className={classes.trackItem}>
                <article className={`${classes.card} ${classes.allCourses}`} aria-disabled="true" aria-describedby="all-courses-status">
                  <span>Todos os cursos</span>
                  <span className={classes.allCoursesArrow} aria-hidden="true">→</span>
                  <span id="all-courses-status" className={classes.visuallyHidden}>Indisponível nesta demonstração.</span>
                </article>
              </li>
            )}
          </ul>
        </HorizontalRail>

        {coursesQuery.isPending && <p role="status">Carregando cursos...</p>}
        {coursesQuery.isError && !coursesQuery.data && (
          <div role="alert">
            <p>{courseErrorMessage(coursesQuery.error)}</p>
            <Button type="button" onClick={() => void coursesQuery.refetch()}>Tentar novamente</Button>
          </div>
        )}
        {coursesQuery.isError && coursesQuery.data && (
          <p role="status">
            {courseErrorMessage(coursesQuery.error)} Os cursos carregados continuam disponíveis.{' '}
            <Button type="button" variant="subtle" onClick={() => void coursesQuery.refetch()}>Tentar novamente</Button>
          </p>
        )}
        {coursesQuery.data?.items.length === 0 && <p role="status">Não há cursos disponíveis no catálogo consultado.</p>}
      </div>
    </section>
  )
}

function CourseCardSkeleton({ animate }: { animate: boolean }) {
  return (
    <div className={`${classes.card} ${classes.skeletonCard}`} aria-hidden="true">
      <Skeleton className={classes.skeletonImage} animate={animate} />
      <Skeleton className={classes.skeletonCategory} animate={animate} />
      <div className={classes.skeletonCopy}>
        <Skeleton className={classes.skeletonTitle} animate={animate} />
        <Skeleton className={classes.skeletonDescription} animate={animate} />
        <Skeleton className={classes.skeletonDescriptionShort} animate={animate} />
      </div>
    </div>
  )
}

function CourseCardView({ course }: { course: CourseSummary }) {
  const categoryIcon = course.category.name === 'Matemática'
    ? assetPaths.categories.mathematics
    : assetPaths.categories.naturalSciences
  const content = (
    <>
      <img className={classes.photo} src={course.imageUrl} alt="" draggable={false} loading="lazy" decoding="async" />
      <span className={classes.gradient} aria-hidden="true" />
      <span className={classes.category}>
        <img src={categoryIcon} alt="" width="20" height="20" />
        <span>{course.category.name}</span>
      </span>
      <span className={classes.copy}>
        <span className={classes.cardTitle}>{course.fullName}</span>
        <span className={classes.description}>{course.shortDescription}</span>
      </span>
    </>
  )

  return (
    <Link className={`${classes.card} ${classes.courseLink}`} to={courseDetailsPath(course.id)}>
      {content}
    </Link>
  )
}
