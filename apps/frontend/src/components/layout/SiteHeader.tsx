import { Burger, Drawer } from '@mantine/core'
import { useDisclosure } from '@mantine/hooks'
import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { assetPaths } from '../../app/assets'
import { routePaths } from '../../app/routes'
import classes from './SiteHeader.module.css'

type SiteHeaderProps = {
  variant: 'landing' | 'details'
}

type NavigationItem = {
  label: string
  href?: string
  kind: 'route' | 'anchor' | 'unavailable'
  appearance?: 'outlined'
}

function getNavigationItems(variant: SiteHeaderProps['variant']): NavigationItem[] {
  return [
    { label: 'Início', href: routePaths.home, kind: 'route' },
    variant === 'landing'
      ? { label: 'Cursos', href: '#cursos', kind: 'anchor' }
      : { label: 'Cursos', kind: 'unavailable' },
    { label: 'Sobre', kind: 'unavailable' },
    { label: 'Área do aluno', kind: 'unavailable', appearance: 'outlined' },
  ]
}

export function SiteHeader({ variant }: SiteHeaderProps) {
  const [menuOpened, menuHandlers] = useDisclosure(false)
  const [isScrolled, setIsScrolled] = useState(false)
  const navigationItems = getNavigationItems(variant)

  useEffect(() => {
    if (variant !== 'landing') return

    const marker = document.getElementById('landing-hero-end')
    if (!marker) return

    const updateScrollState = () => setIsScrolled(marker.getBoundingClientRect().top < 80)
    const observer = new IntersectionObserver(updateScrollState, { rootMargin: '-72px 0px 0px 0px' })

    observer.observe(marker)
    window.addEventListener('scroll', updateScrollState, { passive: true })
    window.addEventListener('resize', updateScrollState)
    updateScrollState()

    return () => {
      observer.disconnect()
      window.removeEventListener('scroll', updateScrollState)
      window.removeEventListener('resize', updateScrollState)
    }
  }, [variant])

  const headerClassName = [
    classes.header,
    variant === 'landing' ? classes.landing : classes.details,
    isScrolled ? classes.scrolled : '',
  ].filter(Boolean).join(' ')

  return (
    <header className={headerClassName}>
      <div className={classes.inner}>
        <Link className={classes.brand} to={routePaths.home} aria-label="SESI Indústria — Início">
          <img
            src={isScrolled || variant === 'details' ? assetPaths.logos.industryPrimary : assetPaths.logos.industryWhite}
            alt="SESI Indústria"
            width="188"
            height="19"
          />
        </Link>

        <nav className={classes.desktopNavigation} aria-label="Navegação principal">
          {navigationItems.map((item) => (
            <NavigationControl key={item.label} item={item} onNavigate={menuHandlers.close} />
          ))}
        </nav>

        <Burger
          className={classes.burger}
          opened={menuOpened}
          onClick={menuHandlers.toggle}
          aria-label={menuOpened ? 'Fechar navegação' : 'Abrir navegação'}
          aria-expanded={menuOpened}
          size="sm"
        />

        <Drawer
          opened={menuOpened}
          onClose={menuHandlers.close}
          title="Navegação"
          position="right"
          size="min(86vw, 22rem)"
          className={classes.drawer}
        >
          <nav className={classes.mobileNavigation} aria-label="Navegação móvel">
            {navigationItems.map((item) => (
              <NavigationControl key={item.label} item={item} onNavigate={menuHandlers.close} />
            ))}
          </nav>
        </Drawer>
      </div>
    </header>
  )
}

function NavigationControl({ item, onNavigate }: { item: NavigationItem; onNavigate: () => void }) {
  if (item.kind === 'route' && item.href) {
    return (
      <Link className={classes.navigationLink} to={item.href} onClick={onNavigate}>
        {item.label}
      </Link>
    )
  }

  if (item.kind === 'anchor' && item.href) {
    return (
      <a className={classes.navigationLink} href={item.href} onClick={onNavigate}>
        {item.label}
      </a>
    )
  }

  return (
    <span
      className={`${classes.navigationLink} ${classes.unavailable} ${item.appearance === 'outlined' ? classes.outlined : ''}`}
      aria-disabled="true"
      aria-label={`${item.label} (indisponível nesta demonstração)`}
    >
      {item.label}
    </span>
  )
}
