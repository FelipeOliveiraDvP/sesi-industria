export type LearningItemState = 'completed' | 'current' | 'pending'

export type LearningItem = {
  label: string
  state: LearningItemState
}

export const learningEnvironment = {
  category: 'CERTIFICAÇÃO',
  courseTitle: 'Matemática para a Precisão: Medidas e Escalas na Metalurgia',
  progressPercent: 10,
  searchLabel: 'Buscar conteúdo do curso',
  introduction: {
    label: '1. Introdução',
    expanded: true,
    items: [
      { label: '1.1 Introdução', state: 'completed' },
      { label: '1.2 Boas práticas', state: 'current' },
      { label: '1.3 Extras', state: 'pending' },
    ] satisfies readonly LearningItem[],
  },
  modules: [
    { label: '2. Módulo 1', expanded: false },
    { label: '3. Módulo 2', expanded: false },
    { label: '4. Módulo 3', expanded: false },
    { label: '5. Módulo 4', expanded: false },
    { label: '6. Módulo 5', expanded: false },
    { label: '7. Módulo 6', expanded: false },
  ],
  article: {
    title: 'Boas práticas',
    paragraphs: [
      'Se você está interessado em se aprofundar em Matemática para a Precisão, especialmente em Medidas e Escalas na Metalurgia, aqui estão algumas dicas para maximizar sua experiência online. Primeiro, crie um espaço de estudo livre de distrações, onde você possa se concentrar totalmente no material.',
      'Estabeleça uma rotina de estudos, reservando horários específicos para assistir às aulas e realizar as atividades. Além disso, interaja com seus colegas e instrutores nos fóruns e chats disponíveis, pois isso enriquece o aprendizado e ajuda a esclarecer dúvidas. Não se esqueça de revisar o conteúdo regularmente e aplicar os conhecimentos em situações práticas sempre que possível.',
      'Por fim, mantenha-se atualizado sobre as novas técnicas e normas em medidas e escalas na metalurgia, já que essa área está sempre evoluindo. Com dedicação e as estratégias corretas, você estará preparado para se destacar no setor!',
    ],
  },
  recommendations: {
    title: 'Recomendações',
    paragraphs: [
      'Se você está interessado em se aprofundar na matemática aplicada à metalurgia, aqui estão algumas dicas valiosas. Primeiro, escolha um curso que ofereça uma abordagem prática, com aulas que incluam exercícios e estudos de caso. Isso ajudará a entender como aplicar medidas e escalas na prática. Além disso, verifique se o curso é reconhecido por instituições relevantes, pois isso pode impactar sua formação e carreira.',
      'Outro aspecto importante é a experiência dos instrutores. Procure cursos que tenham profissionais com experiência no setor, pois eles podem compartilhar insights valiosos que vão além da teoria.',
      'Considere também a carga horária e a flexibilidade do curso. Se você trabalha, um curso que ofereça aulas noturnas ou aos finais de semana pode ser mais conveniente.',
      'Por fim, não se esqueça de se conectar com outros alunos e profissionais da área. Isso pode abrir portas e proporcionar um aprendizado ainda mais enriquecedor. Investir em um bom curso de matemática para a metalurgia é essencial para garantir precisão e eficiência no trabalho.',
    ],
  },
} as const
