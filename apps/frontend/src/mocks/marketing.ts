import { assetPaths } from '../app/assets'

export type CourseCard = {
  category: 'Matemática' | 'Ciências da Natureza'
  categoryIcon: string
  title: string
  description: string
  image: string
  action: 'course-details' | 'unavailable'
}

export const courseCards = [
  {
    category: 'Matemática',
    categoryIcon: assetPaths.categories.mathematics,
    title: 'Matemática para a Precisão: Medidas e Escalas na Metalurgia',
    description: 'Aplicação de grandezas, proporções e escalas em processos de corte, furação e usinagem.',
    image: assetPaths.images.learningBanner,
    action: 'course-details',
  },
  {
    category: 'Matemática',
    categoryIcon: assetPaths.categories.mathematics,
    title: 'Geometria e Modelagem no Design Têxtil',
    description: 'Explora formas geométricas, simetria e padrões matemáticos presentes em tecidos e estampas.',
    image: assetPaths.images.hero,
    action: 'unavailable',
  },
  {
    category: 'Matemática',
    categoryIcon: assetPaths.categories.mathematics,
    title: 'Estatística e Controle de Qualidade na Produção',
    description: 'Leitura e interpretação de dados de controle de qualidade e produtividade.',
    image: assetPaths.images.videoThumbnail,
    action: 'unavailable',
  },
  {
    category: 'Ciências da Natureza',
    categoryIcon: assetPaths.categories.naturalSciences,
    title: 'Física do Movimento e da Energia na Metalurgia',
    description: 'Estudo de forças, atrito, calor e energia em máquinas, fornos e prensas.',
    image: assetPaths.images.videoThumbnail,
    action: 'unavailable',
  },
  {
    category: 'Ciências da Natureza',
    categoryIcon: assetPaths.categories.naturalSciences,
    title: 'Química e Sustentabilidade',
    description: 'Estudo das reações químicas envolvidas na tinturaria e acabamento de tecidos.',
    image: assetPaths.images.videoThumbnail,
    action: 'unavailable',
  },
  {
    category: 'Ciências da Natureza',
    categoryIcon: assetPaths.categories.naturalSciences,
    title: 'Ciência dos Materiais na Metalurgia',
    description: 'Conceitos básicos de estrutura, resistência e propriedades dos metais.',
    image: assetPaths.images.learningBanner,
    action: 'unavailable',
  },
] satisfies readonly CourseCard[]

export type Metric = {
  value: string
  label: string
}

export const impactMetrics = [
  { value: '25.000+', label: 'Profissionais Capacitados' },
  { value: '500+', label: 'Empresas parceiras' },
  { value: '4.8/5', label: 'Avaliação média' },
  { value: '2.5M+', label: 'Horas de capacitação' },
] satisfies readonly Metric[]

export type Benefit = {
  id: string
  title: string
  description: string
  marketingIcon: string
  detailsIcon: string
}

const sharedBenefits = [
  {
    id: 'certification',
    title: 'Certificação SESI',
    description: 'Cursos com a melhor certificação do mercado industrial reconhecidos pelo SESI.',
    marketingIcon: assetPaths.benefits.certificationRed,
    detailsIcon: assetPaths.benefits.certificationPeach,
  },
  {
    id: 'professors',
    title: 'Professores especialistas',
    description: 'Contamos com professores reconhecidos por sua excelência no mercado.',
    marketingIcon: assetPaths.benefits.professorsRed,
    detailsIcon: assetPaths.benefits.professorsPeach,
  },
  {
    id: 'support',
    title: 'Suporte contínuo',
    description: 'Você não está sozinho! Acompanhamento online durante todo o curso.',
    marketingIcon: assetPaths.benefits.supportRed,
    detailsIcon: assetPaths.benefits.supportPeach,
  },
] satisfies readonly Benefit[]

export const landingBenefits = sharedBenefits

export const courseBenefits = [
  ...sharedBenefits,
  {
    id: 'flexibility',
    title: 'Flexibilidade',
    description: 'Turmas presenciais e online com horários que se adaptam à sua rotina.',
    marketingIcon: assetPaths.benefits.flexibilityPeach,
    detailsIcon: assetPaths.benefits.flexibilityPeach,
  },
] satisfies readonly Benefit[]

export type Testimonial = {
  quote: string
  author: string
  role: string
  rating: 5
}

export const testimonials = [
  {
    quote: 'A experiência do curso foi incrível! As aulas foram muito bem estruturadas e o cronograma foi cuidadosamente planejado, facilitando muito o aprendizado.',
    author: 'Lucas Pereira',
    role: 'Analista na Empresa X',
    rating: 5,
  },
  {
    quote: 'Concluir o curso foi uma jornada enriquecedora. As aulas eram dinâmicas e o horário foi organizado para maximizar nosso tempo de estudo.',
    author: 'Mariana Costa',
    role: 'Gestora na Empresa X',
    rating: 5,
  },
  {
    quote: 'O curso superou minhas expectativas! As aulas eram bem elaboradas e o planejamento do horário ajudou a manter o foco e a produtividade.',
    author: 'Thiago Almeida',
    role: 'Técnico na Empresa X',
    rating: 5,
  },
  {
    quote: 'Finalizar o curso foi uma experiência motivadora. As aulas foram muito bem preparadas e o cronograma foi ideal para o aprendizado.',
    author: 'Fernanda Silva',
    role: 'Analista na Empresa X',
    rating: 5,
  },
  {
    quote: 'A conclusão do curso foi uma etapa muito gratificante. As aulas eram bem organizadas e o horário foi pensado para facilitar nosso aprendizado.',
    author: 'Gabriel Santos',
    role: 'Analista na Empresa X',
    rating: 5,
  },
] satisfies readonly Testimonial[]
