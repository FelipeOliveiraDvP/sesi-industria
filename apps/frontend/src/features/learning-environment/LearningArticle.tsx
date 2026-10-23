import { assetPaths } from '../../app/assets'
import { learningEnvironment } from '../../mocks/learning-environment'
import classes from './LearningArticle.module.css'

export function LearningArticle() {
  return (
    <div className={classes.reading}>
      <figure className={classes.banner}>
        <img
          className={classes.bannerImage}
          src={assetPaths.images.learningBanner}
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
        <h1>{learningEnvironment.article.title}</h1>
        {learningEnvironment.article.paragraphs.map((paragraph) => <p key={paragraph}>{paragraph}</p>)}
        <h2>{learningEnvironment.recommendations.title}</h2>
        {learningEnvironment.recommendations.paragraphs.map((paragraph) => <p key={paragraph}>{paragraph}</p>)}
      </article>
    </div>
  )
}
