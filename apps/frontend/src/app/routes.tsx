import { Route, Routes } from 'react-router-dom'
import { AvaPage } from '../pages/AvaPage'
import { CourseDetailsPage } from '../pages/CourseDetailsPage'
import { LandingPage } from '../pages/LandingPage'
import { NotFoundPage } from '../pages/NotFoundPage'
import { RouteEffects } from './RouteEffects'

export const routePaths = {
  home: '/',
  courseDetails: '/cursos/matematica-para-a-precisao',
  learningEnvironment: '/ava/matematica-para-a-precisao',
} as const

export function AppRoutes() {
  return (
    <>
      <RouteEffects />
      <Routes>
        <Route path={routePaths.home} element={<LandingPage />} />
        <Route path={routePaths.courseDetails} element={<CourseDetailsPage />} />
        <Route path={routePaths.learningEnvironment} element={<AvaPage />} />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </>
  )
}
