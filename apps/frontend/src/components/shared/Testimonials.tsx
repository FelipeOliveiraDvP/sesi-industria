import { assetPaths } from '../../app/assets'
import { testimonials } from '../../mocks/marketing'
import { HorizontalRail } from './HorizontalRail'
import classes from './Testimonials.module.css'

type TestimonialsProps = {
  variant: 'landing' | 'details'
}

export function Testimonials({ variant }: TestimonialsProps) {
  const headingId = `testimonials-heading-${variant}`
  const sectionClassName = variant === 'landing' ? classes.landing : classes.details

  return (
    <section className={`${classes.section} ${sectionClassName}`} aria-labelledby={headingId}>
      <div className={classes.container}>
        <HorizontalRail
          label="Depoimentos"
          controlsTone={variant === 'landing' ? 'light' : 'dark'}
          heading={(
            <div className={classes.heading}>
              <p className={classes.eyebrow}>DEPOIMENTOS</p>
              <h2 id={headingId}>Quem fez recomenda</h2>
            </div>
          )}
        >
          <ul className={classes.track}>
            {testimonials.map((testimonial) => (
              <li className={classes.trackItem} key={testimonial.author}>
                <figure className={classes.card}>
                  <blockquote>
                    <p>“{testimonial.quote}”</p>
                  </blockquote>
                  <figcaption className={classes.author}>
                    <strong>{testimonial.author}</strong>
                    <span>{testimonial.role}</span>
                    <span className={classes.stars} role="img" aria-label={`${testimonial.rating} de 5 estrelas`}>
                      {Array.from({ length: testimonial.rating }, (_, index) => (
                        <img key={index} src={assetPaths.controls.star} alt="" width="20" height="20" />
                      ))}
                    </span>
                  </figcaption>
                </figure>
              </li>
            ))}
          </ul>
        </HorizontalRail>
      </div>
    </section>
  )
}
