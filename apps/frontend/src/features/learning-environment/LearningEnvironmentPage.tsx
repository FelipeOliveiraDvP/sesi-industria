import { Burger, Drawer } from '@mantine/core'
import { useDisclosure } from '@mantine/hooks'
import { Link } from 'react-router-dom'
import { routePaths } from '../../app/routes'
import { LearningArticle } from './LearningArticle'
import { LearningSidebar } from './LearningSidebar'
import classes from './LearningEnvironmentPage.module.css'

export function LearningEnvironmentPage() {
  const [drawerOpened, drawerHandlers] = useDisclosure(false)

  return (
    <div className={classes.layout}>
      <div className={classes.desktopSidebar}>
        <LearningSidebar />
      </div>

      <main className={classes.main} aria-describedby="ava-demo-note" tabIndex={-1}>
        <header className={classes.mobileToolbar}>
          <Burger
            opened={drawerOpened}
            onClick={drawerHandlers.toggle}
            aria-label={drawerOpened ? 'Fechar conteúdo do curso' : 'Abrir conteúdo do curso'}
            aria-expanded={drawerOpened}
            size="sm"
          />
          <span className={classes.toolbarTitle}>Prévia do ambiente</span>
          <Link className={classes.mobileBackLink} to={routePaths.courseDetails} aria-label="Voltar para os detalhes do curso">
            <span aria-hidden="true">←</span>
          </Link>
        </header>

        <LearningArticle />
      </main>

      <Drawer
        className={classes.mobileDrawer}
        opened={drawerOpened}
        onClose={drawerHandlers.close}
        title="Conteúdo do curso"
        position="left"
        size="min(86vw, 22rem)"
      >
        <div className={classes.drawerSidebar}>
          <LearningSidebar onReturn={drawerHandlers.close} />
        </div>
      </Drawer>
    </div>
  )
}
