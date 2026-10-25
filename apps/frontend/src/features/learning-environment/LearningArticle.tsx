import { assetPaths } from '../../app/assets'
import type { CourseDetail } from '../courses/types'
import classes from './LearningArticle.module.css'

export function LearningArticle({ course }: { course: CourseDetail }) {
  const learning = course.presentation.learningPreview

  return (
    <div className={classes.reading}>
      <figure className={classes.banner}>
        <img
          className={classes.bannerImage}
          src={learning.bannerImageUrl}
          alt="Mãos usando um instrumento de medição em uma peça metálica."
          fetchPriority="high"
        />
        <span className={classes.bannerOverlay} aria-hidden="true" />
        <img className={classes.bannerLogo} src={assetPaths.logos.sesi} alt="SESI" width="57" height="19" />
      </figure>

      <article className={classes.article} aria-describedby="ava-demo-note">
        <p id="ava-demo-note" className={classes.visuallyHidden}>
          Prévia demonstrativa do ambiente de aprendizagem. O progresso é fixo e nenhum dado de usuário é salvo.
        </p>
        <h1>{learning.article.title}</h1>
        {learning.article.paragraphs.map((paragraph, index) => <p key={`${index}-${paragraph}`}>{paragraph}</p>)}
        <h2>{learning.recommendations.title}</h2>
        {learning.recommendations.paragraphs.map((paragraph, index) => <p key={`${index}-${paragraph}`}>{paragraph}</p>)}
      </article>
    </div>
  )
}
