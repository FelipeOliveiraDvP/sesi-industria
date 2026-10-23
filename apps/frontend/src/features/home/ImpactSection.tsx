import { BenefitList } from '../../components/shared/BenefitList'
import { impactMetrics, landingBenefits } from '../../mocks/marketing'
import classes from './ImpactSection.module.css'

export function ImpactSection() {
  return (
    <section className={classes.section} aria-labelledby="impact-title">
      <div className={classes.container}>
        <div className={classes.layout}>
          <div className={classes.intro}>
            <h2 id="impact-title">Impacto transformador</h2>
            <p>Resultados que demonstram o sucesso da nossa metodologia de capacitação contínua para a indústria</p>
          </div>
          <div className={classes.rightColumn}>
            <dl className={classes.metrics} aria-label="Métricas de impacto demonstrativas">
              {impactMetrics.map((metric) => (
                <div className={classes.metric} key={metric.label}>
                  <dt>{metric.label}</dt>
                  <dd>{metric.value}</dd>
                </div>
              ))}
            </dl>
            <BenefitList items={landingBenefits} variant="marketing" />
          </div>
        </div>
      </div>
    </section>
  )
}
