import { Skeleton } from '@mantine/core'
import { useReducedMotion } from '@mantine/hooks'
import { MarketingLayout } from '../../components/layout/MarketingLayout'
import { BenefitList } from '../../components/shared/BenefitList'
import { Testimonials } from '../../components/shared/Testimonials'
import { courseBenefits } from '../../mocks/marketing'
import { ContactCta } from './ContactCta'
import classes from './CourseDetailsSkeleton.module.css'

export function CourseDetailsSkeleton() {
  const reduceMotion = useReducedMotion()

  return (
    <MarketingLayout variant="details">
      <main className={classes.main} tabIndex={-1} aria-busy="true">
        <p className={classes.status} role="status">Carregando detalhes do curso...</p>

        <section className={classes.hero} aria-hidden="true">
          <div className={classes.heroContainer}>
            <div className={classes.heroCopy}>
              <Skeleton className={classes.badge} animate={!reduceMotion} />
              <div className={classes.title}>
                <Skeleton className={classes.titleLine} animate={!reduceMotion} />
                <Skeleton className={classes.titleLine} animate={!reduceMotion} />
                <Skeleton className={classes.titleLineShort} animate={!reduceMotion} />
              </div>
              <div className={classes.metadata}>
                {[0, 1].map((item) => (
                  <div className={classes.metadataRow} key={item}>
                    <Skeleton width="7rem" height="1rem" animate={!reduceMotion} />
                    <Skeleton width="8rem" height="1rem" animate={!reduceMotion} />
                  </div>
                ))}
              </div>
              <Skeleton className={classes.enrollPlaceholder} animate={!reduceMotion} />
            </div>
            <div className={classes.heroImage}>
              <Skeleton className={classes.imagePlaceholder} animate={!reduceMotion} />
            </div>
          </div>
        </section>

        <section className={classes.benefits} aria-label="Benefícios do curso">
          <div className={classes.container}>
            <BenefitList items={courseBenefits} variant="details" />
          </div>
        </section>

        <section className={classes.content} aria-hidden="true">
          <div className={`${classes.container} ${classes.contentGrid}`}>
            <div>
              <h2>Sobre o curso</h2>
              <div className={classes.descriptionLines}>
                <Skeleton className={classes.descriptionLine} animate={!reduceMotion} />
                <Skeleton className={classes.descriptionLine} animate={!reduceMotion} />
                <Skeleton className={classes.descriptionLine} animate={!reduceMotion} />
                <Skeleton className={classes.descriptionLineShort} animate={!reduceMotion} />
              </div>
            </div>
            <div className={classes.accordions}>
              {[0, 1, 2].map((item) => (
                <div className={classes.accordionRow} key={item}>
                  <Skeleton width="13rem" height="1.125rem" animate={!reduceMotion} />
                  <Skeleton width="1.5rem" height="1.5rem" animate={!reduceMotion} />
                </div>
              ))}
            </div>
          </div>
        </section>

        <Testimonials variant="details" />
        <ContactCta />
      </main>
    </MarketingLayout>
  )
}
