import { assetPaths } from '../../app/assets'
import classes from './SiteFooter.module.css'

const footerColumns = [
  {
    title: 'NAVEGAÇÃO',
    links: ['Início', 'Certificação', 'Cursos', 'Sobre', 'Blog'],
  },
  {
    title: 'CURSOS POPULARES',
    links: [
      'Cultura Organizacional',
      'Ferramentas Google para Produtividade',
      'Excel Básico ao Avançado',
      'Trabalho em Equipe e Colaboração',
      'Gestão do Tempo e Produtividade',
      'Comunicação Assertiva',
      'Inteligência Emocional no Trabalho',
    ],
  },
  {
    title: 'SUPORTE',
    links: ['Termos de Uso', 'Política de Privacidade', 'Trabalhe Conosco', 'Fale Conosco', 'Central de Ajuda'],
  },
] as const

const socialIcons = [
  { name: 'LinkedIn', src: assetPaths.social.linkedIn },
  { name: 'Instagram', src: assetPaths.social.instagram },
  { name: 'Facebook', src: assetPaths.social.facebook },
] as const

export function SiteFooter({ variant }: { variant: 'landing' | 'details' }) {
  return (
    <footer className={`${classes.footer} ${variant === 'details' ? classes.enrollmentSpace : ''}`}>
      <div className={classes.container}>
        <div className={classes.columns}>
          <div className={classes.contactColumn}>
            <img
              className={classes.brand}
              src={assetPaths.logos.industryWhite}
              alt="SESI Indústria"
              width="188"
              height="19"
            />
            <p>contato@sesiindustria.com.br</p>
            <p>Av. Paulista, 1313</p>
          </div>

          {footerColumns.map((column) => (
            <section className={classes.column} key={column.title} aria-label={column.title}>
              <h2>{column.title}</h2>
              <ul>
                {column.links.map((label) => (
                  <li key={label}>
                    <span aria-disabled="true" aria-label={`${label} (indisponível nesta demonstração)`}>
                      {label}
                    </span>
                  </li>
                ))}
              </ul>
            </section>
          ))}
        </div>

        <div className={classes.bottomRow}>
          <div className={classes.copyright}>
            <img src={assetPaths.logos.sesi} alt="SESI" width="34" height="20" />
            <span>Todos os direitos reservados.</span>
          </div>
          <div className={classes.socialIcons} aria-label="Redes sociais">
            {socialIcons.map((icon) => (
              <span key={icon.name} title={icon.name}>
                <img src={icon.src} alt="" width="24" height="24" />
              </span>
            ))}
          </div>
        </div>
      </div>
    </footer>
  )
}
