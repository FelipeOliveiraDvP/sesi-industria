import { Link } from 'react-router-dom'
import { routePaths } from '../../app/routes'
import { VideoPreview } from '../../components/shared/VideoPreview'
import classes from './LandingCtaSection.module.css'

export function LandingCtaSection() {
  return (
    <section className={classes.section} aria-labelledby="landing-cta-title">
      <div className={classes.container}>
        <div className={classes.copy}>
          <h2 id="landing-cta-title">Pronto para transformar sua carreira?</h2>
          <p>Junte-se a milhares de profissionais que já estão transformando a indústria através dos nossos cursos. Invista no seu futuro hoje mesmo!</p>
          <Link className={classes.enrollLink} to={routePaths.courseDetails}>
            Matricule-se <span aria-hidden="true">→</span>
          </Link>
        </div>
        <VideoPreview description="Prévia de vídeo ilustrativa. A reprodução não está disponível nesta demonstração." />
      </div>
    </section>
  )
}
