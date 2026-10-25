import { LearningEnvironmentPage } from '../features/learning-environment/LearningEnvironmentPage'

export function AvaPage({ courseId }: { courseId: number }) {
  return <LearningEnvironmentPage key={courseId} courseId={courseId} />
}
