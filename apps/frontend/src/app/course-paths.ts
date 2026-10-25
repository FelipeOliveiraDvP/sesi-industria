const MAX_COURSE_ID = 2_147_483_647

export function parseCourseId(value: string | undefined): number | null {
  if (value === undefined || !/^\d+$/.test(value)) return null

  const id = Number(value)
  return Number.isSafeInteger(id) && id > 0 && id <= MAX_COURSE_ID ? id : null
}

function requireCourseId(value: number): number {
  if (!Number.isSafeInteger(value) || value < 1 || value > MAX_COURSE_ID) {
    throw new RangeError('Course ID must be a positive 32-bit integer.')
  }
  return value
}

export function courseDetailsPath(courseId: number): string {
  return `/cursos/${requireCourseId(courseId)}`
}

export function learningEnvironmentPath(courseId: number): string {
  return `/ava/${requireCourseId(courseId)}`
}
