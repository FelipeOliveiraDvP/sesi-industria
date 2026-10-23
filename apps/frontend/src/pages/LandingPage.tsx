import { MarketingLayout } from '../components/layout/MarketingLayout'
import { HeroSection } from '../features/home/HeroSection'
import { CourseDiscoverySection } from '../features/home/CourseDiscoverySection'
import { ImpactSection } from '../features/home/ImpactSection'
import { Testimonials } from '../components/shared/Testimonials'
import { LandingCtaSection } from '../features/home/LandingCtaSection'

export function LandingPage() {
  return (
    <MarketingLayout variant="landing">
      <main tabIndex={-1}>
        <HeroSection />
        <CourseDiscoverySection />
        <ImpactSection />
        <Testimonials variant="landing" />
        <LandingCtaSection />
      </main>
    </MarketingLayout>
  )
}
