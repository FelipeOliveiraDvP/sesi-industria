import classes from './ContactCta.module.css'

export function ContactCta() {
  return (
    <section className={classes.section} aria-labelledby="contact-cta-title">
      <p className={classes.eyebrow}>PRECISA DE AJUDA PARA DECIDIR?</p>
      <h2 id="contact-cta-title">Fale com a gente</h2>
      <button type="button" className={classes.button} disabled aria-label="Mandar uma mensagem (indisponível nesta demonstração)">
        Mandar uma mensagem <span aria-hidden="true">→</span>
      </button>
    </section>
  )
}
