import { Link } from 'react-router-dom'
import { useId, useState } from 'react'
import { courseDetailsPath } from '../../app/course-paths'
import type { CourseDetail, LearningItemState } from '../courses/types'
import classes from './LearningSidebar.module.css'

type LearningSidebarProps = {
  course: CourseDetail
  onReturn?: () => void
}

const stateLabels: Record<LearningItemState, string> = {
  completed: 'Concluída',
  current: 'Lição atual',
  pending: 'Pendente',
} as const

export function LearningSidebar({ course, onReturn }: LearningSidebarProps) {
  const learning = course.presentation.learningPreview
  const moduleListId = useId()
  const [expandedModules, setExpandedModules] = useState(() =>
    new Set(learning.modules.filter((module) => module.expanded).map((module) => module.label)),
  )

  const toggleModule = (label: string) => {
    setExpandedModules((current) => {
      const next = new Set(current)
      if (next.has(label)) next.delete(label)
      else next.add(label)
      return next
    })
  }

  return (
    <aside className={classes.sidebar} aria-label="Conteúdo do ambiente de aprendizagem">
      <div className={classes.identity}>
        <Link className={classes.backLink} to={courseDetailsPath(course.id)} onClick={onReturn} aria-label="Voltar para os detalhes do curso">
          <span aria-hidden="true">←</span>
        </Link>
        <p className={classes.category}>{learning.category}</p>
        <h2>{course.fullName}</h2>
        <div className={classes.progressRow}>
          <div
            className={classes.progressTrack}
            role="progressbar"
            aria-label="Progresso demonstrativo do curso"
            aria-valuemin={0}
            aria-valuemax={100}
            aria-valuenow={learning.progressPercent}
          >
            <span style={{ width: `${learning.progressPercent}%` }} />
          </div>
          <span className={classes.progressLabel}>{learning.progressPercent}%</span>
        </div>
      </div>

      <nav className={classes.navigation} aria-label="Conteúdo do curso">
        <label className={classes.search}>
          <span className={classes.visuallyHidden}>{learning.searchLabel}; indisponível nesta demonstração</span>
          <input type="search" disabled aria-label={`${learning.searchLabel}, indisponível nesta demonstração`} />
          <svg aria-hidden="true" viewBox="0 0 24 24" focusable="false">
            <circle cx="10.5" cy="10.5" r="6.5" />
            <path d="m15.5 15.5 5 5" />
          </svg>
        </label>

        <div className={classes.introduction}>
          <h3>{learning.introduction.label}</h3>
          <ul className={classes.lessonItems}>
            {learning.introduction.items.map((item) => (
              <li className={item.state === 'current' ? classes.currentItem : ''} key={item.label}>
                <LessonStatus state={item.state} />
                <span>{item.label}</span>
              </li>
            ))}
          </ul>
        </div>

        <ul className={classes.moduleItems}>
          {learning.modules.map((module, index) => {
            const expanded = expandedModules.has(module.label)
            const lessonsId = `${moduleListId}-lessons-${index}`
            return (
              <li className={classes.moduleItem} key={module.label}>
                <button
                  className={classes.moduleButton}
                  type="button"
                  aria-expanded={expanded}
                  aria-controls={lessonsId}
                  onClick={() => toggleModule(module.label)}
                >
                  <span>{module.label}</span>
                  <span
                    className={`${classes.moduleChevron} ${expanded ? classes.moduleChevronExpanded : ''}`}
                    aria-hidden="true"
                  />
                </button>
                <ul className={classes.moduleLessons} id={lessonsId} hidden={!expanded}>
                  {module.lessons.map((lesson) => (
                    <li className={classes.moduleLesson} key={lesson.label}>
                      <span className={classes.moduleLessonLabel}>{lesson.label}</span>
                      <p>{lesson.description}</p>
                    </li>
                  ))}
                </ul>
              </li>
            )
          })}
        </ul>
      </nav>
    </aside>
  )
}

function LessonStatus({ state }: { state: keyof typeof stateLabels }) {
  return (
    <span className={`${classes.status} ${classes[state]}`} role="img" aria-label={stateLabels[state]}>
      {state === 'completed' && <span aria-hidden="true">✓</span>}
      {state === 'current' && <span className={classes.playTriangle} aria-hidden="true" />}
    </span>
  )
}
