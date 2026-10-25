import axios from 'axios'
import { ApiError, apiClient, assertApiConfiguration, toApiError } from '../../lib/api-client'
import type {
  CourseAssessmentSection,
  CourseDetail,
  CourseFaq,
  CourseLesson,
  CourseListResponse,
  CourseLookup,
  CourseModule,
  CoursePresentation,
  CourseSummary,
  CourseTextSection,
  LearningArticle,
  LearningIntroduction,
  LearningItem,
  LearningItemState,
  LearningPreview,
  LearningSidebarModule,
  LearningSidebarLesson,
} from './types'

function isRecord(value: unknown): value is Record<string, unknown> {
  return typeof value === 'object' && value !== null && !Array.isArray(value)
}

function invalidResponse(path: string): ApiError {
  return new ApiError('invalid-response', `The course API returned an incompatible response (${path}).`)
}

function readRecord(value: unknown, path: string): Record<string, unknown> {
  if (!isRecord(value)) throw invalidResponse(path)
  return value
}

function readText(record: Record<string, unknown>, key: string, path: string): string {
  const value = record[key]
  if (typeof value !== 'string' || value.trim() === '') throw invalidResponse(path)
  return value
}

function readInteger(record: Record<string, unknown>, key: string, path: string): number {
  const value = record[key]
  if (typeof value !== 'number' || !Number.isSafeInteger(value)) throw invalidResponse(path)
  return value
}

function readPositiveId(record: Record<string, unknown>, key: string, path: string): number {
  const value = readInteger(record, key, path)
  if (value < 1) throw invalidResponse(path)
  return value
}

function readBoolean(record: Record<string, unknown>, key: string, path: string): boolean {
  const value = record[key]
  if (typeof value !== 'boolean') throw invalidResponse(path)
  return value
}

function readArray(record: Record<string, unknown>, key: string, path: string): unknown[] {
  const value = record[key]
  if (!Array.isArray(value)) throw invalidResponse(path)
  return value
}

function readPublicImageUrl(record: Record<string, unknown>, key: string, path: string): string {
  const value = readText(record, key, path)
  try {
    const url = new URL(value)
    if (
      (url.protocol !== 'http:' && url.protocol !== 'https:') ||
      url.username !== '' ||
      url.password !== '' ||
      !url.pathname.startsWith('/assets/') ||
      url.pathname.includes('..')
    ) {
      throw new Error('Invalid image URL')
    }
  } catch {
    throw invalidResponse(path)
  }
  return value
}

function readLookup(value: unknown, path: string): CourseLookup {
  const record = readRecord(value, path)
  return {
    id: readPositiveId(record, 'id', `${path}.id`),
    name: readText(record, 'name', `${path}.name`),
  }
}

function readSummary(value: unknown, path: string): CourseSummary {
  const record = readRecord(value, path)
  const availableOn = record.availableOn
  if (availableOn !== null &&
      (typeof availableOn !== 'string' || !/^\d{4}-\d{2}-\d{2}$/.test(availableOn))) {
    throw invalidResponse(`${path}.availableOn`)
  }
  const workloadHours = record.workloadHours
  if (typeof workloadHours !== 'number' || !Number.isFinite(workloadHours)) {
    throw invalidResponse(`${path}.workloadHours`)
  }

  return {
    id: readPositiveId(record, 'id', `${path}.id`),
    fullName: readText(record, 'fullName', `${path}.fullName`),
    shortName: readText(record, 'shortName', `${path}.shortName`),
    description: readText(record, 'description', `${path}.description`),
    shortDescription: readText(record, 'shortDescription', `${path}.shortDescription`),
    category: readLookup(record.category, `${path}.category`),
    modality: readLookup(record.modality, `${path}.modality`),
    status: readLookup(record.status, `${path}.status`),
    workloadHours,
    targetAudience: readText(record, 'targetAudience', `${path}.targetAudience`),
    imageUrl: readPublicImageUrl(record, 'imageUrl', `${path}.imageUrl`),
    availableOn,
  }
}

function readTextArray(record: Record<string, unknown>, key: string, path: string): string[] {
  return readArray(record, key, path).map((value, index) => {
    if (typeof value !== 'string' || value.trim() === '') throw invalidResponse(`${path}[${index}]`)
    return value
  })
}

function readArticle(value: unknown, path: string): LearningArticle {
  const record = readRecord(value, path)
  return {
    title: readText(record, 'title', `${path}.title`),
    paragraphs: readTextArray(record, 'paragraphs', `${path}.paragraphs`),
  }
}

function readLearningItem(value: unknown, path: string): LearningItem {
  const record = readRecord(value, path)
  const state = readText(record, 'state', `${path}.state`)
  if (state !== 'completed' && state !== 'current' && state !== 'pending') {
    throw invalidResponse(`${path}.state`)
  }
  return {
    label: readText(record, 'label', `${path}.label`),
    state: state as LearningItemState,
  }
}

function readPresentation(value: unknown, path: string): CoursePresentation {
  const record = readRecord(value, path)
  const online = readRecord(record.onlineLessons, `${path}.onlineLessons`)
  const assessments = readRecord(record.assessments, `${path}.assessments`)
  const faqs: CourseFaq[] = readArray(record, 'faqs', `${path}.faqs`).map((faq, index) => {
    const faqRecord = readRecord(faq, `${path}.faqs[${index}]`)
    return {
      question: readText(faqRecord, 'question', `${path}.faqs[${index}].question`),
      answer: readText(faqRecord, 'answer', `${path}.faqs[${index}].answer`),
    }
  })

  const learningRecord = readRecord(record.learningPreview, `${path}.learningPreview`)
  const introductionRecord = readRecord(learningRecord.introduction, `${path}.learningPreview.introduction`)
  const progressPercent = readInteger(learningRecord, 'progressPercent', `${path}.learningPreview.progressPercent`)
  if (progressPercent < 0 || progressPercent > 100) throw invalidResponse(`${path}.learningPreview.progressPercent`)

  const introduction: LearningIntroduction = {
    label: readText(introductionRecord, 'label', `${path}.learningPreview.introduction.label`),
    expanded: readBoolean(introductionRecord, 'expanded', `${path}.learningPreview.introduction.expanded`),
    items: readArray(introductionRecord, 'items', `${path}.learningPreview.introduction.items`)
      .map((item, index) => readLearningItem(item, `${path}.learningPreview.introduction.items[${index}]`)),
  }
  const sidebarModules: LearningSidebarModule[] = readArray(learningRecord, 'modules', `${path}.learningPreview.modules`)
    .map((module, index) => {
      const moduleRecord = readRecord(module, `${path}.learningPreview.modules[${index}]`)
      const modulePath = `${path}.learningPreview.modules[${index}]`
      const lessons: LearningSidebarLesson[] = readArray(moduleRecord, 'lessons', `${modulePath}.lessons`)
        .map((lesson, lessonIndex) => {
          const lessonPath = `${modulePath}.lessons[${lessonIndex}]`
          const lessonRecord = readRecord(lesson, lessonPath)
          return {
            label: readText(lessonRecord, 'label', `${lessonPath}.label`),
            description: readText(lessonRecord, 'description', `${lessonPath}.description`),
          }
        })
      if (lessons.length !== 2) throw invalidResponse(`${modulePath}.lessons`)
      return {
        label: readText(moduleRecord, 'label', `${modulePath}.label`),
        expanded: readBoolean(moduleRecord, 'expanded', `${modulePath}.expanded`),
        lessons,
      }
    })

  const learningPreview: LearningPreview = {
    category: readText(learningRecord, 'category', `${path}.learningPreview.category`),
    progressPercent,
    searchLabel: readText(learningRecord, 'searchLabel', `${path}.learningPreview.searchLabel`),
    bannerImageUrl: readPublicImageUrl(learningRecord, 'bannerImageUrl', `${path}.learningPreview.bannerImageUrl`),
    introduction,
    modules: sidebarModules,
    article: readArticle(learningRecord.article, `${path}.learningPreview.article`),
    recommendations: readArticle(learningRecord.recommendations, `${path}.learningPreview.recommendations`),
  }

  const onlineLessons: CourseTextSection = {
    introduction: readText(online, 'introduction', `${path}.onlineLessons.introduction`),
    topics: readTextArray(online, 'topics', `${path}.onlineLessons.topics`),
    accessibility: readText(online, 'accessibility', `${path}.onlineLessons.accessibility`),
  }
  const assessmentSection: CourseAssessmentSection = {
    introduction: readText(assessments, 'introduction', `${path}.assessments.introduction`),
    topics: readTextArray(assessments, 'topics', `${path}.assessments.topics`),
    conclusion: readText(assessments, 'conclusion', `${path}.assessments.conclusion`),
  }

  return {
    certification: readText(record, 'certification', `${path}.certification`),
    detailsImageUrl: readPublicImageUrl(record, 'detailsImageUrl', `${path}.detailsImageUrl`),
    onlineLessons,
    assessments: assessmentSection,
    faqs,
    learningPreview,
  }
}

function readModule(value: unknown, path: string): CourseModule {
  const record = readRecord(value, path)
  const lessons: CourseLesson[] = readArray(record, 'lessons', `${path}.lessons`).map((lesson, index) => {
    const lessonRecord = readRecord(lesson, `${path}.lessons[${index}]`)
    return {
      id: readPositiveId(lessonRecord, 'id', `${path}.lessons[${index}].id`),
      title: readText(lessonRecord, 'title', `${path}.lessons[${index}].title`),
      description: readText(lessonRecord, 'description', `${path}.lessons[${index}].description`),
      order: readInteger(lessonRecord, 'order', `${path}.lessons[${index}].order`),
    }
  })
  return {
    id: readPositiveId(record, 'id', `${path}.id`),
    title: readText(record, 'title', `${path}.title`),
    description: readText(record, 'description', `${path}.description`),
    order: readInteger(record, 'order', `${path}.order`),
    lessons,
  }
}

export function parseCourseListResponse(value: unknown): CourseListResponse {
  const record = readRecord(value, 'course list')
  const items: CourseSummary[] = readArray(record, 'items', 'items').map((item, index) =>
    readSummary(item, `items[${index}]`),
  )
  const totalCount = readInteger(record, 'totalCount', 'totalCount')
  if (totalCount < 0 || totalCount !== items.length) throw invalidResponse('totalCount')
  return { items, totalCount }
}

export function parseCourseDetail(value: unknown, expectedId: number): CourseDetail {
  const record = readRecord(value, 'course detail')
  const summary = readSummary(record, 'course detail')
  if (summary.id !== expectedId) throw invalidResponse('course detail id')

  return {
    ...summary,
    modules: readArray(record, 'modules', 'modules').map((module, index) => readModule(module, `modules[${index}]`)),
    presentation: readPresentation(record.presentation, 'presentation'),
  }
}

async function get<T>(path: string, signal: AbortSignal, parse: (value: unknown) => T): Promise<T> {
  try {
    assertApiConfiguration()
    const response = await apiClient.get<unknown>(path, { signal })
    return parse(response.data)
  } catch (error) {
    throw toApiError(error)
  }
}

export function getCourses(signal: AbortSignal): Promise<CourseListResponse> {
  return get('/api/courses', signal, parseCourseListResponse)
}

export function getCourse(id: number, signal: AbortSignal): Promise<CourseDetail> {
  if (!Number.isSafeInteger(id) || id < 1) {
    return Promise.reject(new ApiError('configuration', 'A positive integer course ID is required.'))
  }
  return get(`/api/courses/${id}`, signal, (value) => parseCourseDetail(value, id))
}

export function isRequestCancelled(error: unknown): boolean {
  return axios.isCancel(error) || (error instanceof ApiError && error.kind === 'cancelled')
}
