import { Accordion } from '@mantine/core'
import { useState } from 'react'
import { assetPaths } from '../../app/assets'
import { course, courseAssessments, courseFaqs, courseModules, onlineLessons } from '../../mocks/course-details'
import classes from './CourseDetailsContent.module.css'

export function CourseDetailsContent() {
  const [openGroups, setOpenGroups] = useState<string[]>([])

  return (
    <section className={classes.section} aria-labelledby="about-course-title">
      <div className={classes.container}>
        <div className={classes.layout}>
          <div className={classes.about}>
            <h2 id="about-course-title">Sobre o curso</h2>
            <p>{course.description}</p>
          </div>

          <Accordion
            className={classes.accordion}
            classNames={{
              item: classes.accordionItem,
              control: classes.accordionControl,
              label: classes.accordionLabel,
              chevron: classes.accordionChevron,
              panel: classes.accordionPanel,
            }}
            multiple
            value={openGroups}
            onChange={setOpenGroups}
            order={3}
            variant="default"
            disableChevronRotation
            aria-label="Informações do curso"
          >
            <Accordion.Item value="modules">
              <Accordion.Control chevron={<AccordionChevron opened={openGroups.includes('modules')} />}>
                Módulos
              </Accordion.Control>
              <Accordion.Panel>
                <ol className={classes.modules}>
                  {courseModules.map((module, index) => (
                    <li key={module.title}>
                      <h4>{index + 1}. {module.title}</h4>
                      <ul>
                        {module.topics.map((topic) => <li key={topic}>{topic}</li>)}
                      </ul>
                    </li>
                  ))}
                </ol>
              </Accordion.Panel>
            </Accordion.Item>

            <Accordion.Item value="lessons">
              <Accordion.Control chevron={<AccordionChevron opened={openGroups.includes('lessons')} />}>
                Aulas online e avaliações
              </Accordion.Control>
              <Accordion.Panel>
                <div className={classes.contentBlock}>
                  <h4>Aulas online</h4>
                  <p>{onlineLessons.introduction}</p>
                  <ul>
                    {onlineLessons.topics.map((topic) => <li key={topic}>{topic}</li>)}
                  </ul>
                  <p>{onlineLessons.accessibility}</p>
                </div>
                <div className={classes.contentBlock}>
                  <h4>Provas e Avaliações</h4>
                  <p>{courseAssessments.introduction}</p>
                  <ul>
                    {courseAssessments.topics.map((topic) => <li key={topic}>{topic}</li>)}
                  </ul>
                  <p>{courseAssessments.conclusion}</p>
                </div>
              </Accordion.Panel>
            </Accordion.Item>

            <Accordion.Item value="faq">
              <Accordion.Control chevron={<AccordionChevron opened={openGroups.includes('faq')} />}>
                Dúvidas frequentes
              </Accordion.Control>
              <Accordion.Panel>
                <dl className={classes.faqList}>
                  {courseFaqs.map((faq) => (
                    <div key={faq.question}>
                      <dt>{faq.question}</dt>
                      <dd>{faq.answer}</dd>
                    </div>
                  ))}
                </dl>
              </Accordion.Panel>
            </Accordion.Item>
          </Accordion>
        </div>
      </div>
    </section>
  )
}

function AccordionChevron({ opened }: { opened: boolean }) {
  return (
    <img
      src={opened ? assetPaths.controls.collapse : assetPaths.controls.expand}
      alt=""
      width="24"
      height="24"
    />
  )
}
