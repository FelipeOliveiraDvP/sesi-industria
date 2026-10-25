import { useQuery } from '@tanstack/react-query'
import { getCourse, getCourses } from './api'

export const courseQueryKeys = {
  all: ['courses'] as const,
  list: () => ['courses', 'list'] as const,
  details: () => ['courses', 'detail'] as const,
  detail: (id: number) => ['courses', 'detail', normalizeCourseId(id)] as const,
}

export function normalizeCourseId(id: number): number {
  return Number.isSafeInteger(id) && id > 0 ? id : 0
}

export function useCourses() {
  return useQuery({
    queryKey: courseQueryKeys.list(),
    queryFn: ({ signal }) => getCourses(signal),
  })
}

export function useCourse(courseId: number) {
  const id = normalizeCourseId(courseId)
  return useQuery({
    queryKey: courseQueryKeys.detail(id),
    queryFn: ({ signal }) => getCourse(id, signal),
    enabled: id > 0,
  })
}
