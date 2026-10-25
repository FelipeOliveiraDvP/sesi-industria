import { Navigate, Route, Routes, useParams } from 'react-router-dom'
import { AvaPage } from '../pages/AvaPage'
import { CourseDetailsPage } from '../pages/CourseDetailsPage'
import { LandingPage } from '../pages/LandingPage'
import { NotFoundPage } from '../pages/NotFoundPage'
import { courseDetailsPath, learningEnvironmentPath, parseCourseId } from './course-paths'
import { RouteEffects } from './RouteEffects'

export const routePaths = {
  home: '/',
  courseDetails: courseDetailsPath(1),
  learningEnvironment: learningEnvironmentPath(1),
  legacyCourseDetails: '/cursos/matematica-para-a-precisao',
  legacyLearningEnvironment: '/ava/matematica-para-a-precisao',
} as const

function CourseDetailsRoute() {
  const { courseId } = useParams()
  const id = parseCourseId(courseId)
  return id === null ? <NotFoundPage /> : <CourseDetailsPage courseId={id} />
}

function LearningEnvironmentRoute() {
  const { courseId } = useParams()
  const id = parseCourseId(courseId)
  return id === null ? <NotFoundPage /> : <AvaPage courseId={id} />
}

export function AppRoutes() {
  return (
    <>
      <RouteEffects />
      <Routes>
        <Route path={routePaths.home} element={<LandingPage />} />
        <Route path={routePaths.legacyCourseDetails} element={<Navigate to={routePaths.courseDetails} replace />} />
        <Route path={routePaths.legacyLearningEnvironment} element={<Navigate to={routePaths.learningEnvironment} replace />} />
        <Route path="/cursos/:courseId" element={<CourseDetailsRoute />} />
        <Route path="/ava/:courseId" element={<LearningEnvironmentRoute />} />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </>
  )
}
