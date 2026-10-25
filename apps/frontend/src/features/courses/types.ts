export type CourseLookup = {
  id: number
  name: string
}

export type CourseSummary = {
  id: number
  fullName: string
  shortName: string
  description: string
  shortDescription: string
  category: CourseLookup
  modality: CourseLookup
  status: CourseLookup
  workloadHours: number
  targetAudience: string
  imageUrl: string
  availableOn: string | null
}

export type CourseListResponse = {
  items: CourseSummary[]
  totalCount: number
}

export type CourseLesson = {
  id: number
  title: string
  description: string
  order: number
}

export type CourseModule = {
  id: number
  title: string
  description: string
  order: number
  lessons: CourseLesson[]
}

export type CourseFaq = {
  question: string
  answer: string
}

export type CourseTextSection = {
  introduction: string
  topics: string[]
  accessibility: string
}

export type CourseAssessmentSection = {
  introduction: string
  topics: string[]
  conclusion: string
}

export type LearningItemState = 'completed' | 'current' | 'pending'

export type LearningItem = {
  label: string
  state: LearningItemState
}

export type LearningIntroduction = {
  label: string
  expanded: boolean
  items: LearningItem[]
}

export type LearningSidebarModule = {
  label: string
  expanded: boolean
  lessons: LearningSidebarLesson[]
}

export type LearningSidebarLesson = {
  label: string
  description: string
}

export type LearningArticle = {
  title: string
  paragraphs: string[]
}

export type LearningPreview = {
  category: string
  progressPercent: number
  searchLabel: string
  bannerImageUrl: string
  introduction: LearningIntroduction
  modules: LearningSidebarModule[]
  article: LearningArticle
  recommendations: LearningArticle
}

export type CoursePresentation = {
  certification: string
  detailsImageUrl: string
  onlineLessons: CourseTextSection
  assessments: CourseAssessmentSection
  faqs: CourseFaq[]
  learningPreview: LearningPreview
}

export type CourseDetail = CourseSummary & {
  modules: CourseModule[]
  presentation: CoursePresentation
}
