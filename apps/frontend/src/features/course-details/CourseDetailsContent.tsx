import { Accordion } from '@mantine/core'
import { useState } from 'react'
import { assetPaths } from '../../app/assets'
import type { CourseDetail } from '../courses/types'
import classes from './CourseDetailsContent.module.css'

export function CourseDetailsContent({ course }: { course: CourseDetail }) {
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
                  {course.modules.map((module) => (
                    <li key={module.id}>
                      <h4>{module.order}. {module.title}</h4>
                      <ul>
                        {module.lessons.map((lesson) => <li key={lesson.id}>{lesson.description}</li>)}
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
                  <p>{course.presentation.onlineLessons.introduction}</p>
                  <ul>
                    {course.presentation.onlineLessons.topics.map((topic) => <li key={topic}>{topic}</li>)}
                  </ul>
                  <p>{course.presentation.onlineLessons.accessibility}</p>
                </div>
                <div className={classes.contentBlock}>
                  <h4>Provas e Avaliações</h4>
                  <p>{course.presentation.assessments.introduction}</p>
                  <ul>
                    {course.presentation.assessments.topics.map((topic) => <li key={topic}>{topic}</li>)}
                  </ul>
                  <p>{course.presentation.assessments.conclusion}</p>
                </div>
              </Accordion.Panel>
            </Accordion.Item>

            <Accordion.Item value="faq">
              <Accordion.Control chevron={<AccordionChevron opened={openGroups.includes('faq')} />}>
                Dúvidas frequentes
              </Accordion.Control>
              <Accordion.Panel>
                <dl className={classes.faqList}>
                  {course.presentation.faqs.map((faq) => (
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
