import { Link } from 'react-router-dom'
import { assetPaths } from '../../app/assets'
import { learningEnvironmentPath } from '../../app/course-paths'
import { courseBenefits } from '../../mocks/marketing'
import { BenefitList } from '../../components/shared/BenefitList'
import { VideoPreview } from '../../components/shared/VideoPreview'
import { formatCourseDate } from '../courses/format'
import type { CourseDetail } from '../courses/types'
import classes from './CourseDetailsHero.module.css'

export function CourseDetailsHero({ course }: { course: CourseDetail }) {
  const categoryIcon = course.category.name === 'Matemática'
    ? assetPaths.categories.mathematics
    : assetPaths.categories.naturalSciences

  return (
    <>
      <section className={classes.hero} aria-labelledby="course-title">
        <div className={classes.container}>
          <div className={classes.copy}>
            <span className={classes.badge}>
              <img src={categoryIcon} alt="" width="20" height="20" />
              {course.category.name}
            </span>
            <h1 id="course-title">{course.fullName}</h1>
            <dl className={classes.metadata}>
              <div>
                <dt>Início do curso</dt>
                <dd><MetadataIcon kind="calendar" />{formatCourseDate(course.availableOn)}</dd>
              </div>
              <div>
                <dt>Certificação</dt>
                <dd><MetadataIcon kind="certificate" />{course.presentation.certification}</dd>
              </div>
            </dl>
            <Link className={classes.enrollLink} to={learningEnvironmentPath(course.id)} aria-describedby="enrollment-demo-note">
              Matricule-se <span aria-hidden="true">→</span>
            </Link>
            <p id="enrollment-demo-note" className={classes.visuallyHidden}>Abre uma prévia demonstrativa do ambiente de aprendizagem. Nenhuma matrícula é realizada.</p>
          </div>

          <VideoPreview imageUrl={course.imageUrl} imageAlt="Imagem ilustrativa de uma atividade industrial."
            description="Prévia ilustrativa do curso. A reprodução de vídeo não está disponível nesta demonstração." />
        </div>
      </section>
      <div id="course-hero-end" className={classes.marker} aria-hidden="true" />

      <section className={classes.benefits} aria-label="Benefícios do curso">
        <div className={classes.container}>
          <BenefitList items={courseBenefits} variant="details" />
        </div>
      </section>
    </>
  )
}

function MetadataIcon({ kind }: { kind: 'calendar' | 'certificate' }) {
  if (kind === 'calendar') {
    return (
      <svg aria-hidden="true" viewBox="0 0 24 24" focusable="false">
        <rect x="3.5" y="5" width="17" height="15" rx="2" />
        <path d="M7.5 3.5v3M16.5 3.5v3M4 9h16M8 12.5h2M8 16h2M14 12.5h2" />
      </svg>
    )
  }

  return (
    <svg aria-hidden="true" viewBox="0 0 24 24" focusable="false">
      <circle cx="12" cy="9" r="5.5" />
      <path d="m8.5 14-1 7 4.5-2.5 4.5 2.5-1-7M9.5 9h5M12 6.5v5" />
    </svg>
  )
}
