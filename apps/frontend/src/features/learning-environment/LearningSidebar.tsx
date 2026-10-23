import { Link } from 'react-router-dom'
import { routePaths } from '../../app/routes'
import { learningEnvironment } from '../../mocks/learning-environment'
import classes from './LearningSidebar.module.css'

type LearningSidebarProps = {
  onReturn?: () => void
}

const stateLabels = {
  completed: 'Concluída',
  current: 'Lição atual',
  pending: 'Pendente',
} as const

export function LearningSidebar({ onReturn }: LearningSidebarProps) {
  return (
    <aside className={classes.sidebar} aria-label="Conteúdo do ambiente de aprendizagem">
      <div className={classes.identity}>
        <Link className={classes.backLink} to={routePaths.courseDetails} onClick={onReturn} aria-label="Voltar para os detalhes do curso">
          <span aria-hidden="true">←</span>
        </Link>
        <p className={classes.category}>{learningEnvironment.category}</p>
        <h2>{learningEnvironment.courseTitle}</h2>
        <div className={classes.progressRow}>
          <div
            className={classes.progressTrack}
            role="progressbar"
            aria-label="Progresso demonstrativo do curso"
            aria-valuemin={0}
            aria-valuemax={100}
            aria-valuenow={learningEnvironment.progressPercent}
          >
            <span style={{ width: `${learningEnvironment.progressPercent}%` }} />
          </div>
          <span className={classes.progressLabel}>{learningEnvironment.progressPercent}%</span>
        </div>
      </div>

      <nav className={classes.navigation} aria-label="Conteúdo do curso">
        <label className={classes.search}>
          <span className={classes.visuallyHidden}>{learningEnvironment.searchLabel}; indisponível nesta demonstração</span>
          <input type="search" disabled aria-label={`${learningEnvironment.searchLabel}, indisponível nesta demonstração`} />
          <svg aria-hidden="true" viewBox="0 0 24 24" focusable="false">
            <circle cx="10.5" cy="10.5" r="6.5" />
            <path d="m15.5 15.5 5 5" />
          </svg>
        </label>

        <div className={classes.introduction}>
          <h3>{learningEnvironment.introduction.label}</h3>
          <ul className={classes.lessonItems}>
            {learningEnvironment.introduction.items.map((item) => (
              <li className={item.state === 'current' ? classes.currentItem : ''} key={item.label}>
                <LessonStatus state={item.state} />
                <span>{item.label}</span>
              </li>
            ))}
          </ul>
        </div>

        <ul className={classes.moduleItems}>
          {learningEnvironment.modules.map((module) => (
            <li key={module.label} aria-disabled="true">{module.label}</li>
          ))}
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
