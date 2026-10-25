import { Skeleton } from '@mantine/core'
import { useReducedMotion } from '@mantine/hooks'
import { Link } from 'react-router-dom'
import { courseDetailsPath } from '../../app/course-paths'
import pageClasses from './LearningEnvironmentPage.module.css'
import sidebarClasses from './LearningSidebar.module.css'
import articleClasses from './LearningArticle.module.css'
import classes from './LearningEnvironmentSkeleton.module.css'

export function LearningEnvironmentSkeleton({ courseId }: { courseId: number }) {
  const reduceMotion = useReducedMotion()
  const detailsPath = courseDetailsPath(courseId)

  return (
    <div className={pageClasses.layout}>
      <div className={pageClasses.desktopSidebar}>
        <aside className={sidebarClasses.sidebar} aria-label="Conteúdo do ambiente de aprendizagem" aria-busy="true">
          <div className={sidebarClasses.identity}>
            <Link className={sidebarClasses.backLink} to={detailsPath} aria-label="Voltar para os detalhes do curso">
              <span aria-hidden="true">←</span>
            </Link>
            <div className={classes.identitySkeleton} aria-hidden="true">
              <Skeleton className={classes.category} animate={!reduceMotion} />
              <Skeleton className={classes.courseTitle} animate={!reduceMotion} />
              <Skeleton className={classes.courseTitleShort} animate={!reduceMotion} />
              <div className={classes.progressRow}>
                <Skeleton className={classes.progressTrack} animate={!reduceMotion} />
                <Skeleton className={classes.progressLabel} animate={!reduceMotion} />
              </div>
            </div>
          </div>

          <nav className={sidebarClasses.navigation} aria-hidden="true">
            <div className={classes.sidebarContent}>
              <Skeleton className={classes.search} animate={!reduceMotion} />
              <div className={classes.introduction}>
                <Skeleton width="6rem" height="0.875rem" animate={!reduceMotion} />
                {[0, 1, 2].map((item) => (
                  <div className={classes.lessonRow} key={item}>
                    <Skeleton circle width="1rem" height="1rem" animate={!reduceMotion} />
                    <Skeleton width={`${7 + item % 2}rem`} height="0.75rem" animate={!reduceMotion} />
                  </div>
                ))}
              </div>
              <div className={classes.moduleRows}>
                {[0, 1, 2, 3, 4, 5].map((item) => (
                  <div className={classes.moduleRow} key={item}>
                    <Skeleton width={`${7 + (item % 3)}rem`} height="0.75rem" animate={!reduceMotion} />
                  </div>
                ))}
              </div>
            </div>
          </nav>
        </aside>
      </div>

      <main className={pageClasses.main} tabIndex={-1}>
        <header className={pageClasses.mobileToolbar}>
          <span className={pageClasses.toolbarTitle}>Prévia do ambiente</span>
          <Link className={pageClasses.mobileBackLink} to={detailsPath} aria-label="Voltar para os detalhes do curso">
            <span aria-hidden="true">←</span>
          </Link>
        </header>
        <p className={classes.status} role="status">Carregando a prévia do curso...</p>

        <div className={articleClasses.reading} aria-busy="true" aria-hidden="true">
          <figure className={`${articleClasses.banner} ${classes.banner}`}>
            <Skeleton className={classes.bannerSkeleton} animate={!reduceMotion} />
          </figure>
          <div className={`${articleClasses.article} ${classes.articleSkeleton}`}>
            <Skeleton className={classes.articleHeading} animate={!reduceMotion} />
            <ArticleParagraph animate={!reduceMotion} />
            <ArticleParagraph animate={!reduceMotion} />
            <ArticleParagraph animate={!reduceMotion} />
            <Skeleton className={classes.recommendationsHeading} animate={!reduceMotion} />
            <ArticleParagraph animate={!reduceMotion} />
            <ArticleParagraph animate={!reduceMotion} />
          </div>
        </div>
      </main>
    </div>
  )
}

function ArticleParagraph({ animate }: { animate: boolean }) {
  return (
    <div className={classes.articleParagraph}>
      <Skeleton className={classes.paragraphLine} animate={animate} />
      <Skeleton className={classes.paragraphLine} animate={animate} />
      <Skeleton className={classes.paragraphLineShort} animate={animate} />
    </div>
  )
}
