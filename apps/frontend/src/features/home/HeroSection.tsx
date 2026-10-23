import { Button } from '@mantine/core'
import { assetPaths } from '../../app/assets'
import classes from './HeroSection.module.css'

export function HeroSection() {
  return (
    <section className={classes.hero} aria-labelledby="landing-title">
      <img className={classes.photo} src={assetPaths.images.hero} alt="" aria-hidden="true" fetchPriority="high" />
      <div className={classes.overlay} aria-hidden="true" />
      <div className={classes.container}>
        <div className={classes.copy}>
          <h1 id="landing-title">Impulsione sua carreira na indústria</h1>
          <p>
            Amplie seus conhecimentos com excelência na melhor plataforma de estudos da indústria brasileira
          </p>
          <Button component="a" href="#cursos" className={classes.cta} radius="sm" size="md">
            Conhecer cursos <span aria-hidden="true">→</span>
          </Button>
        </div>
      </div>
      <div id="landing-hero-end" className={classes.marker} aria-hidden="true" />
    </section>
  )
}
