import { Button } from '@mantine/core'
import { useId } from 'react'
import { Link } from 'react-router-dom'
import { HorizontalRail } from '../../components/shared/HorizontalRail'
import { routePaths } from '../../app/routes'
import { courseCards, type CourseCard } from '../../mocks/marketing'
import classes from './CourseDiscoverySection.module.css'

export function CourseDiscoverySection() {
  return (
    <section id="cursos" tabIndex={-1} className={classes.section} aria-labelledby="courses-heading">
      <div className={classes.container}>
        <HorizontalRail
          label="Cursos em destaque"
          heading={<h2 id="courses-heading" className={classes.heading}>Principais cursos</h2>}
        >
          <ul className={classes.track}>
            {courseCards.map((course) => (
              <li className={classes.trackItem} key={course.title}>
                <CourseCardView course={course} />
              </li>
            ))}
            <li className={classes.trackItem}>
              <article className={`${classes.card} ${classes.allCourses}`} aria-disabled="true" aria-describedby="all-courses-status">
                <span>Todos os cursos</span>
                <span className={classes.allCoursesArrow} aria-hidden="true">→</span>
                <span id="all-courses-status" className={classes.visuallyHidden}>Indisponível nesta demonstração.</span>
              </article>
            </li>
          </ul>
        </HorizontalRail>

        <div className={classes.allCoursesAction}>
          <Button type="button" className={classes.allCoursesButton} disabled aria-label="Ver todos os cursos (indisponível nesta demonstração)">
            Ver todos os cursos
          </Button>
        </div>
      </div>
    </section>
  )
}

function CourseCardView({ course }: { course: CourseCard }) {
  const unavailableId = useId()
  const content = (
    <>
      <img className={classes.photo} src={course.image} alt="" loading="lazy" decoding="async" />
      <span className={classes.gradient} aria-hidden="true" />
      <span className={classes.category}>
        <img src={course.categoryIcon} alt="" width="20" height="20" />
        <span>{course.category}</span>
      </span>
      <span className={classes.copy}>
        <span className={classes.cardTitle}>{course.title}</span>
        <span className={classes.description}>{course.description}</span>
      </span>
    </>
  )

  if (course.action === 'course-details') {
    return (
      <Link className={`${classes.card} ${classes.courseLink}`} to={routePaths.courseDetails}>
        {content}
      </Link>
    )
  }

  return (
    <article className={classes.card} aria-disabled="true" aria-describedby={unavailableId}>
      {content}
      <span id={unavailableId} className={classes.visuallyHidden}>Indisponível nesta demonstração.</span>
    </article>
  )
}
