import type { ReactNode } from 'react'
import { SiteFooter } from './SiteFooter'
import { SiteHeader } from './SiteHeader'

type MarketingLayoutProps = {
  children: ReactNode
  variant: 'landing' | 'details'
}

export function MarketingLayout({ children, variant }: MarketingLayoutProps) {
  return (
    <div>
      <SiteHeader variant={variant} />
      {children}
      <SiteFooter variant={variant} />
    </div>
  )
}
