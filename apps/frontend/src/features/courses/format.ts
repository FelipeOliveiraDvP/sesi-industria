export function formatCourseDate(availableOn: string | null): string {
  if (availableOn === null) return 'A definir'

  const match = /^(\d{4})-(\d{2})-(\d{2})$/.exec(availableOn)
  if (!match) return 'A definir'

  const [, year, month, day] = match
  return `${day}/${month}/${year}`
}
