export type CourseModule = {
  title: string
  topics: readonly string[]
}

export const course = {
  category: 'Matemática',
  title: 'Matemática para a Precisão: Medidas e Escalas na Metalurgia',
  startDate: '05/01/2026',
  certification: 'SESI',
  description:
    'Explore nosso curso de Matemática para a Precisão: Medidas e Escalas na Metalurgia, perfeito para quem deseja se destacar nesse campo! Este programa abrangente foi desenvolvido para capacitar profissionais com as habilidades e conhecimentos fundamentais para garantir medições precisas e eficientes no ambiente metalúrgico.',
} as const

export const courseModules = [
  {
    title: 'Introdução à Matemática na Metalurgia',
    topics: [
      'Conceitos fundamentais de medidas e escalas aplicados à metalurgia.',
      'Importância da precisão nas medições e controle de qualidade.',
      'Panorama das normas e regulamentos no Brasil relacionados à metalurgia.',
    ],
  },
  {
    title: 'Identificação e Avaliação de Medidas',
    topics: [
      'Tipos de medidas utilizadas: comprimento, massa, volume e temperatura.',
      'Técnicas de análise de precisão nas medições.',
      'Elaboração de gráficos e tabelas de medidas.',
    ],
  },
  {
    title: 'Normas e Regulamentações',
    topics: [
      'Estudo das principais normas aplicáveis à metalurgia (ABNT, ISO, entre outras).',
      'Interpretação e aplicação prática das normas de medição.',
      'Atualizações e impactos legais nas práticas de medição.',
    ],
  },
  {
    title: 'Instrumentos de Medição',
    topics: [
      'Tipos de instrumentos utilizados na metalurgia: paquímetros, micrômetros e balanças.',
      'Critérios de seleção, uso e calibração.',
      'Responsabilidades legais de operadores e supervisores.',
    ],
  },
  {
    title: 'Prevenção de Erros de Medição',
    topics: [
      'Tipos de erros e incertezas nas medições.',
      'Procedimentos para garantir a precisão e exatidão.',
      'Treinamento prático em técnicas de medição.',
    ],
  },
  {
    title: 'Ergonomia e Eficiência na Medição',
    topics: [
      'Princípios ergonômicos aplicados ao uso de instrumentos de medição.',
      'Prevenção de lesões por uso inadequado de ferramentas.',
      'Qualidade de vida no ambiente de trabalho.',
    ],
  },
  {
    title: 'Análise de Resultados de Medição',
    topics: [
      'Técnicas de análise de dados de medições.',
      'Elaboração de relatórios e planos de ação para melhorias.',
      'Indicadores de precisão e melhoria contínua.',
    ],
  },
  {
    title: 'Cultura de Precisão e Comportamento Seguro',
    topics: [
      'Desenvolvimento de uma cultura organizacional voltada à precisão nas medições.',
      'Comunicação eficaz e liderança em práticas de medição.',
      'Programas de conscientização e engajamento dos colaboradores.',
    ],
  },
  {
    title: 'Tecnologias Aplicadas à Medição',
    topics: [
      'Uso de plataformas digitais, sensores e automação na medição de precisão.',
      'Sistemas de gestão de qualidade.',
      'Tendências e inovações no setor metalúrgico.',
    ],
  },
  {
    title: 'Avaliação Final e Certificação',
    topics: [
      'Teste de conhecimentos adquiridos sobre medidas e escalas.',
      'Atividades práticas e estudos de caso.',
      'Emissão de certificado digital de conclusão.',
    ],
  },
] satisfies readonly CourseModule[]

export const onlineLessons = {
  introduction: 'As aulas são organizadas em módulos temáticos e disponibilizadas em formato multimídia, combinando:',
  topics: [
    'Vídeo-aulas com especialistas da área, gravadas em alta qualidade e com recursos visuais que facilitam a compreensão dos conteúdos técnicos.',
    'Materiais complementares como apostilas digitais, infográficos, artigos e estudos de caso.',
    'Atividades interativas, como quizzes, simulações e fóruns de discussão, que promovem o engajamento e a troca de experiências entre os participantes.',
    'Acesso multiplataforma, permitindo que os alunos estudem pelo computador, tablet ou celular, a qualquer hora e de qualquer lugar.',
  ],
  accessibility:
    'Cada aula é acompanhada de recursos de acessibilidade, como legendas, transcrição de áudio e navegação intuitiva, garantindo inclusão e autonomia para todos os perfis de usuários.',
} as const

export const courseAssessments = {
  introduction: 'As avaliações são realizadas diretamente na plataforma, com foco na verificação do aprendizado e na aplicação prática dos conhecimentos adquiridos:',
  topics: [
    'Provas objetivas ao final de cada módulo, com correção automática e feedback imediato.',
    'Estudos de caso e atividades práticas, que simulam situações reais da indústria e exigem análise crítica e tomada de decisão.',
    'Avaliação final integradora, que consolida os conteúdos do curso e valida a competência do aluno para atuar com segurança no ambiente industrial.',
    'Sistema antifraude, com controle de tempo, embaralhamento de questões e, opcionalmente, reconhecimento facial para garantir a integridade das avaliações.',
  ],
  conclusion:
    'Ao concluir o curso e ser aprovado nas avaliações, o aluno recebe um certificado digital reconhecido, que pode ser compartilhado em redes profissionais e utilizado para comprovação de capacitação técnica.',
} as const

export type CourseFaq = {
  question: string
  answer: string
}

export const courseFaqs = [
  {
    question: 'O curso é totalmente online?',
    answer: 'Sim. Todas as aulas, atividades e avaliações são realizadas pela plataforma digital, permitindo que o aluno estude de onde estiver, no seu próprio ritmo.',
  },
  {
    question: 'Preciso estar disponível em horários específicos?',
    answer: 'Não. As aulas são gravadas e ficam disponíveis 24 horas por dia. O aluno pode acessar o conteúdo quando for mais conveniente, respeitando os prazos de cada módulo.',
  },
  {
    question: 'Como são feitas as avaliações?',
    answer: 'As avaliações são realizadas diretamente na plataforma, por meio de provas objetivas, estudos de caso e atividades práticas. Algumas avaliações podem ter tempo limitado e exigem atenção às instruções.',
  },
  {
    question: 'O curso oferece certificado?',
    answer: 'Sim. Ao concluir todos os módulos e ser aprovado nas avaliações, o aluno recebe um certificado digital de conclusão, válido em todo o território nacional.',
  },
  {
    question: 'Quem são os professores?',
    answer: 'O corpo docente é formado por especialistas, engenheiros e professores com ampla experiência em segurança do trabalho e atuação direta na indústria. Os perfis dos docentes estão disponíveis na plataforma.',
  },
  {
    question: 'Há suporte para dúvidas durante o curso?',
    answer: 'Sim. Os alunos podem enviar dúvidas por meio dos fóruns de discussão, chats da plataforma ou durante os webinários ao vivo. A equipe pedagógica está disponível para auxiliar durante todo o curso.',
  },
  {
    question: 'O curso exige algum conhecimento prévio?',
    answer: 'Não é obrigatório, mas é recomendado que o aluno tenha interesse ou alguma familiaridade com o ambiente industrial. O curso foi estruturado para atender tanto iniciantes quanto profissionais em busca de atualização.',
  },
  {
    question: 'Posso acessar o curso pelo celular?',
    answer: 'Sim. A plataforma é responsiva e compatível com computadores, tablets e smartphones, garantindo uma experiência completa em qualquer dispositivo.',
  },
  {
    question: 'Quanto tempo tenho para concluir o curso?',
    answer: 'O prazo para conclusão varia conforme o plano escolhido. Em geral, o aluno tem entre 3 a 6 meses para finalizar todos os módulos e avaliações.',
  },
  {
    question: 'O curso é reconhecido por órgãos oficiais?',
    answer: 'O curso segue as diretrizes das Normas Regulamentadoras (NRs) e boas práticas da área, sendo reconhecido como formação complementar. Para certificações oficiais, é necessário verificar os requisitos específicos de cada órgão regulador.',
  },
] satisfies readonly CourseFaq[]
