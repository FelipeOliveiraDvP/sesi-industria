import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { learningEnvironmentPath } from '../../app/course-paths'
import type { CourseDetail } from '../courses/types'
import classes from './EnrollmentBar.module.css'

export function EnrollmentBar({ course }: { course: CourseDetail }) {
  const [visible, setVisible] = useState(false)

  useEffect(() => {
    const marker = document.getElementById('course-hero-end')
    if (!marker) return

    const updateVisibility = () => setVisible(marker.getBoundingClientRect().top < 0)
    const observer = new IntersectionObserver(updateVisibility)

    observer.observe(marker)
    window.addEventListener('scroll', updateVisibility, { passive: true })
    window.addEventListener('resize', updateVisibility)
    updateVisibility()

    return () => {
      observer.disconnect()
      window.removeEventListener('scroll', updateVisibility)
      window.removeEventListener('resize', updateVisibility)
    }
  }, [])

  return (
    <div
      id="enrollment-bar"
      className={`${classes.bar} ${visible ? classes.visible : classes.hidden}`}
      role="region"
      aria-label="Matrícula demonstrativa do curso"
      aria-hidden={!visible}
      inert={!visible}
    >
      <p>{course.fullName}</p>
      <Link className={classes.link} to={learningEnvironmentPath(course.id)} aria-describedby="enrollment-bar-demo-note">
        Matricule-se <span aria-hidden="true">→</span>
      </Link>
      <span id="enrollment-bar-demo-note" className={classes.visuallyHidden}>
        Abre uma prévia demonstrativa do ambiente de aprendizagem. Nenhuma matrícula é realizada.
      </span>
    </div>
  )
}
