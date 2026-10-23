import { MarketingLayout } from '../components/layout/MarketingLayout'
import { CourseDetailsHero } from '../features/course-details/CourseDetailsHero'
import { CourseDetailsContent } from '../features/course-details/CourseDetailsContent'
import { EnrollmentBar } from '../features/course-details/EnrollmentBar'
import { ContactCta } from '../features/course-details/ContactCta'
import { Testimonials } from '../components/shared/Testimonials'

export function CourseDetailsPage() {
  return (
    <MarketingLayout variant="details">
      <main tabIndex={-1}>
        <CourseDetailsHero />
        <CourseDetailsContent />
        <Testimonials variant="details" />
        <ContactCta />
        <EnrollmentBar />
      </main>
    </MarketingLayout>
  )
}
