using Catalog.Api.Content;
using Catalog.Api.Entities;

namespace Catalog.Api.Data.Seed;

public static class CatalogSeedData
{
    public static Category[] CreateCategories() =>
    [
        new() { Id = 1, Name = "Matemática" },
        new() { Id = 2, Name = "Ciências da Natureza" },
        new() { Id = 3, Name = "Linguagens" }
    ];

    public static Modality[] CreateModalities() =>
    [
        new() { Id = 1, Name = "Online" },
        new() { Id = 2, Name = "Presencial" }
    ];

    public static CourseStatus[] CreateStatuses() =>
    [
        new() { Id = 1, Name = "Disponível" },
        new() { Id = 2, Name = "Em breve" },
        new() { Id = 3, Name = "Encerrado" }
    ];

    public static Course[] CreateCourses() =>
    [
        BuildCourse(
            id: 1, fullName: "Matemática para a Precisão: Medidas e Escalas na Metalurgia",
            shortName: "Metrologia",
            shortDescription: "Aplicação de grandezas, proporções e escalas em processos de corte, furação e usinagem.",
            description: "Explore nosso curso de Matemática para a Precisão: Medidas e Escalas na Metalurgia, perfeito para quem deseja se destacar nesse campo! Este programa abrangente foi desenvolvido para capacitar profissionais com as habilidades e conhecimentos fundamentais para garantir medições precisas e eficientes no ambiente metalúrgico.",
            categoryId: 1, modalityId: 1, statusId: 1, workloadHours: 40,
            targetAudience: "Operadores metalúrgicos e técnicos em formação", availableOn: new DateOnly(2026, 1, 5),
            modules: BuildMathModules()),
        BuildCourse(
            id: 2, fullName: "Geometria e Modelagem no Design Têxtil", shortName: "GeoTêxtil",
            shortDescription: "Explora formas geométricas, simetria e padrões matemáticos presentes em tecidos e estampas.",
            description: "Explore como formas geométricas, simetria e proporções orientam a criação de tecidos e estampas. O curso relaciona conceitos matemáticos a medidas, modelagem e planificação para apoiar profissionais do design e da produção têxtil.",
            categoryId: 1, modalityId: 2, statusId: 1, workloadHours: 32,
            targetAudience: "Modelistas e profissionais de produção têxtil", availableOn: null,
            modules: BuildTextileGeometryModules()),
        BuildCourse(
            id: 3, fullName: "Estatística e Controle de Qualidade na Produção", shortName: "QualiDados",
            shortDescription: "Leitura e interpretação de dados de controle de qualidade e produtividade.",
            description: "Aprenda a interpretar dados de qualidade e produtividade em processos industriais. O curso apresenta conceitos de amostragem, variação e acompanhamento de processos para apoiar decisões de inspetores e lideranças de produção.",
            categoryId: 1, modalityId: 1, statusId: 2, workloadHours: 24,
            targetAudience: "Inspetores de qualidade e líderes de produção", availableOn: new DateOnly(2027, 2, 10),
            modules: BuildQualityControlModules()),
        BuildCourse(
            id: 4, fullName: "Física do Movimento e da Energia na Metalurgia", shortName: "Física Industrial",
            shortDescription: "Estudo de forças, atrito, calor e energia em máquinas, fornos e prensas.",
            description: "Relacione movimento, forças, atrito, calor e energia às máquinas, fornos e prensas da indústria metalúrgica. A formação conecta conceitos de Física a situações de operação e manutenção industrial.",
            categoryId: 2, modalityId: 2, statusId: 3, workloadHours: 36,
            targetAudience: "Operadores e técnicos de manutenção industrial", availableOn: new DateOnly(2025, 8, 20),
            modules: BuildIndustrialPhysicsModules()),
        BuildCourse(
            id: 5, fullName: "Química e Sustentabilidade", shortName: "Química Têxtil",
            shortDescription: "Estudo das reações químicas envolvidas na tinturaria e acabamento de tecidos.",
            description: "Conheça as reações químicas presentes na tinturaria e no acabamento de tecidos, relacionando processos, uso de recursos e tratamento de resíduos. O conteúdo aborda práticas de sustentabilidade aplicadas à produção têxtil.",
            categoryId: 2, modalityId: 1, statusId: 2, workloadHours: 20,
            targetAudience: "Profissionais de tinturaria e gestão ambiental", availableOn: new DateOnly(2027, 3, 5),
            modules: BuildTextileChemistryModules()),
        BuildCourse(
            id: 6, fullName: "Ciência dos Materiais na Metalurgia", shortName: "Materiais Industriais",
            shortDescription: "Conceitos básicos de estrutura, resistência e propriedades dos metais.",
            description: "Explore a estrutura, a resistência e outras propriedades dos metais para compreender sua seleção e aplicação em processos metalúrgicos. O curso apresenta conceitos de materiais, tratamentos e inspeção para equipes técnicas.",
            categoryId: 2, modalityId: 2, statusId: 3, workloadHours: 30,
            targetAudience: "Técnicos metalúrgicos e inspetores de materiais", availableOn: null,
            modules: BuildMaterialsScienceModules())
    ];

    private static CourseModule[] BuildMathModules() =>
    [
        BuildModule(1, 1, "Introdução à Matemática na Metalurgia", "Fundamentos de medidas e escalas em processos metalúrgicos.",
            ("Tópico 1", "Conceitos fundamentais de medidas e escalas aplicados à metalurgia."),
            ("Tópico 2", "Importância da precisão nas medições e controle de qualidade."),
            ("Tópico 3", "Panorama das normas e regulamentos no Brasil relacionados à metalurgia.")),
        BuildModule(1, 2, "Identificação e Avaliação de Medidas", "Tipos de medidas e interpretação dos resultados obtidos.",
            ("Tópico 1", "Tipos de medidas utilizadas: comprimento, massa, volume e temperatura."),
            ("Tópico 2", "Técnicas de análise de precisão nas medições."),
            ("Tópico 3", "Elaboração de gráficos e tabelas de medidas.")),
        BuildModule(1, 3, "Normas e Regulamentações", "Aplicação das normas relevantes à medição industrial.",
            ("Tópico 1", "Estudo das principais normas aplicáveis à metalurgia (ABNT, ISO, entre outras)."),
            ("Tópico 2", "Interpretação e aplicação prática das normas de medição."),
            ("Tópico 3", "Atualizações e impactos legais nas práticas de medição.")),
        BuildModule(1, 4, "Instrumentos de Medição", "Seleção, uso e calibração de instrumentos metalúrgicos.",
            ("Tópico 1", "Tipos de instrumentos utilizados na metalurgia: paquímetros, micrômetros e balanças."),
            ("Tópico 2", "Critérios de seleção, uso e calibração."),
            ("Tópico 3", "Responsabilidades legais de operadores e supervisores.")),
        BuildModule(1, 5, "Prevenção de Erros de Medição", "Reconhecimento de incertezas e práticas para reduzir erros.",
            ("Tópico 1", "Tipos de erros e incertezas nas medições."),
            ("Tópico 2", "Procedimentos para garantir a precisão e exatidão."),
            ("Tópico 3", "Treinamento prático em técnicas de medição.")),
        BuildModule(1, 6, "Ergonomia e Eficiência na Medição", "Uso ergonômico de instrumentos e cuidados no trabalho.",
            ("Tópico 1", "Princípios ergonômicos aplicados ao uso de instrumentos de medição."),
            ("Tópico 2", "Prevenção de lesões por uso inadequado de ferramentas."),
            ("Tópico 3", "Qualidade de vida no ambiente de trabalho.")),
        BuildModule(1, 7, "Análise de Resultados de Medição", "Análise de dados e melhoria contínua dos processos.",
            ("Tópico 1", "Técnicas de análise de dados de medições."),
            ("Tópico 2", "Elaboração de relatórios e planos de ação para melhorias."),
            ("Tópico 3", "Indicadores de precisão e melhoria contínua.")),
        BuildModule(1, 8, "Cultura de Precisão e Comportamento Seguro", "Práticas de equipe que favorecem precisão e segurança.",
            ("Tópico 1", "Desenvolvimento de uma cultura organizacional voltada à precisão nas medições."),
            ("Tópico 2", "Comunicação eficaz e liderança em práticas de medição."),
            ("Tópico 3", "Programas de conscientização e engajamento dos colaboradores.")),
        BuildModule(1, 9, "Tecnologias Aplicadas à Medição", "Tecnologias digitais e tendências de medição industrial.",
            ("Tópico 1", "Uso de plataformas digitais, sensores e automação na medição de precisão."),
            ("Tópico 2", "Sistemas de gestão de qualidade."),
            ("Tópico 3", "Tendências e inovações no setor metalúrgico.")),
        BuildModule(1, 10, "Avaliação Final e Certificação", "Consolidação dos conhecimentos sobre medidas e escalas.",
            ("Tópico 1", "Teste de conhecimentos adquiridos sobre medidas e escalas."),
            ("Tópico 2", "Atividades práticas e estudos de caso."),
            ("Tópico 3", "Emissão de certificado digital de conclusão."))
    ];

    private static CourseModule[] BuildTextileGeometryModules() =>
    [
        BuildModule(2, 1, "Formas e simetrias", "Leitura de formas geométricas em tecidos e estampas.",
            ("Formas no tecido", "Identificação de formas geométricas em composições têxteis."),
            ("Eixos de simetria", "Análise de simetria em motivos e padrões de estampas.")),
        BuildModule(2, 2, "Medidas e proporções", "Relação entre medidas, escala e proporção no design.",
            ("Medidas de referência", "Seleção de unidades e medidas para desenvolver um modelo têxtil."),
            ("Escalas de ampliação", "Uso de escalas para ampliar ou reduzir elementos de uma composição."),
            ("Proporção visual", "Comparação de proporções entre formas, repetição e área do tecido.")),
        BuildModule(2, 3, "Modelagem e planificação", "Conversão de formas em moldes e superfícies planas.",
            ("Leitura de moldes", "Interpretação das medidas e formas representadas em um molde."),
            ("Planificação de superfícies", "Organização de formas planas para orientar o corte de materiais.")),
        BuildModule(2, 4, "Aplicação em estampas", "Aplicação de padrões geométricos em composições têxteis.",
            ("Repetição de motivos", "Planejamento da repetição de formas em uma estampa contínua."),
            ("Composição final", "Revisão de alinhamento, escala e simetria em uma proposta de estampa."))
    ];

    private static CourseModule[] BuildQualityControlModules() =>
    [
        BuildModule(3, 1, "Leitura de dados", "Interpretação de registros de qualidade e produtividade.",
            ("Tabelas de produção", "Leitura de tabelas com medições e volumes de produção."),
            ("Gráficos de controle", "Interpretação de gráficos usados para acompanhar a qualidade.")),
        BuildModule(3, 2, "Variação e amostragem", "Variação dos processos e seleção de amostras.",
            ("Variação do processo", "Identificação de variações comuns em medições de produção."),
            ("Amostragem", "Seleção de amostras para observar características de um lote."),
            ("Resumo estatístico", "Uso de medidas descritivas para sintetizar dados de qualidade.")),
        BuildModule(3, 3, "Controle de processos", "Acompanhamento de indicadores e ações de melhoria.",
            ("Indicadores de qualidade", "Escolha de indicadores para acompanhar a conformidade de produtos."),
            ("Ação corretiva", "Organização de evidências para apoiar uma ação de melhoria no processo."))
    ];

    private static CourseModule[] BuildIndustrialPhysicsModules() =>
    [
        BuildModule(4, 1, "Movimento", "Descrição do movimento de máquinas e componentes industriais.",
            ("Trajetórias e deslocamento", "Descrição de trajetórias e deslocamentos em equipamentos industriais."),
            ("Velocidade de operação", "Relação entre distância, tempo e velocidade em uma operação.")),
        BuildModule(4, 2, "Forças e atrito", "Forças e atrito em operações de máquinas e prensas.",
            ("Forças em equipamentos", "Reconhecimento das forças envolvidas no funcionamento de máquinas."),
            ("Atrito entre superfícies", "Análise do atrito em componentes e superfícies de contato.")),
        BuildModule(4, 3, "Trabalho e potência", "Trabalho, potência e transferência de energia mecânica.",
            ("Trabalho mecânico", "Relação entre força, deslocamento e trabalho em uma tarefa industrial."),
            ("Potência de máquinas", "Interpretação da potência indicada para equipamentos industriais."),
            ("Consumo de energia", "Comparação do consumo de energia em diferentes condições de operação.")),
        BuildModule(4, 4, "Calor e transferência", "Calor e transferência térmica em fornos e materiais.",
            ("Temperatura e calor", "Distinção entre temperatura e calor em processos metalúrgicos."),
            ("Transferência térmica", "Identificação de condução, convecção e radiação em equipamentos.")),
        BuildModule(4, 5, "Eficiência energética", "Leitura de perdas e oportunidades de eficiência energética.",
            ("Perdas de energia", "Reconhecimento de perdas energéticas em máquinas e fornos."),
            ("Uso eficiente", "Discussão de práticas para melhorar o uso de energia na operação."))
    ];

    private static CourseModule[] BuildTextileChemistryModules() =>
    [
        BuildModule(5, 1, "Reações e processos têxteis", "Química das etapas de tinturaria e acabamento.",
            ("Reações de tinturaria", "Descrição das reações químicas envolvidas na tinturaria de tecidos."),
            ("Banhos e parâmetros", "Leitura de parâmetros de processo em banhos de tingimento."),
            ("Acabamento têxtil", "Relação entre agentes químicos e propriedades do acabamento têxtil.")),
        BuildModule(5, 2, "Uso de recursos e tratamento de resíduos", "Práticas de recursos e resíduos em processos químicos.",
            ("Consumo de água e insumos", "Observação do uso de água e insumos em processos de produção têxtil."),
            ("Tratamento de efluentes", "Etapas gerais de tratamento de efluentes gerados na tinturaria."),
            ("Prevenção de resíduos", "Alternativas para reduzir resíduos e apoiar práticas sustentáveis."))
    ];

    private static CourseModule[] BuildMaterialsScienceModules() =>
    [
        BuildModule(6, 1, "Estrutura dos metais", "Estruturas metálicas e sua relação com as propriedades.",
            ("Organização interna", "Introdução à organização interna dos materiais metálicos."),
            ("Estruturas e aplicações", "Relação entre estruturas dos metais e suas aplicações industriais.")),
        BuildModule(6, 2, "Propriedades mecânicas", "Propriedades mecânicas relevantes à seleção de materiais.",
            ("Resistência dos materiais", "Leitura de conceitos de resistência em componentes metálicos."),
            ("Dureza e ductilidade", "Comparação entre dureza, ductilidade e comportamento dos metais."),
            ("Ensaios mecânicos", "Visão geral de ensaios usados para avaliar propriedades mecânicas.")),
        BuildModule(6, 3, "Tratamentos e seleção", "Tratamentos e critérios de seleção de materiais.",
            ("Tratamentos térmicos", "Descrição dos objetivos gerais de tratamentos térmicos em metais."),
            ("Seleção de materiais", "Comparação de propriedades para selecionar um material para uma aplicação.")),
        BuildModule(6, 4, "Inspeção de materiais", "Inspeção e registros de materiais em operações metalúrgicas.",
            ("Inspeção visual", "Identificação de características observáveis durante a inspeção de materiais."),
            ("Registro de resultados", "Organização de registros de inspeção para apoiar a rastreabilidade."))
    ];

    private static CoursePresentation BuildPresentation(int courseId) => courseId switch
    {
        1 => CreatePresentation(
            onlineIntroduction: "As aulas são organizadas em módulos temáticos e disponibilizadas em formato multimídia, combinando:",
            onlineTopics:
            [
                "Vídeo-aulas com especialistas da área, gravadas em alta qualidade e com recursos visuais que facilitam a compreensão dos conteúdos técnicos.",
                "Materiais complementares como apostilas digitais, infográficos, artigos e estudos de caso.",
                "Atividades interativas, como quizzes, simulações e fóruns de discussão, que promovem o engajamento e a troca de experiências entre os participantes.",
                "Acesso multiplataforma, permitindo que os alunos estudem pelo computador, tablet ou celular, a qualquer hora e de qualquer lugar."
            ],
            onlineAccessibility: "Cada aula é acompanhada de recursos de acessibilidade, como legendas, transcrição de áudio e navegação intuitiva, garantindo inclusão e autonomia para todos os perfis de usuários.",
            assessmentIntroduction: "As avaliações são realizadas diretamente na plataforma, com foco na verificação do aprendizado e na aplicação prática dos conhecimentos adquiridos:",
            assessmentTopics:
            [
                "Provas objetivas ao final de cada módulo, com correção automática e feedback imediato.",
                "Estudos de caso e atividades práticas, que simulam situações reais da indústria e exigem análise crítica e tomada de decisão.",
                "Avaliação final integradora, que consolida os conteúdos do curso e valida a competência do aluno para atuar com segurança no ambiente industrial.",
                "Sistema antifraude, com controle de tempo, embaralhamento de questões e, opcionalmente, reconhecimento facial para garantir a integridade das avaliações."
            ],
            assessmentConclusion: "Ao concluir o curso e ser aprovado nas avaliações, o aluno recebe um certificado digital reconhecido, que pode ser compartilhado em redes profissionais e utilizado para comprovação de capacitação técnica.",
            faqs: CreateMathematicsFaqs(),
            articleParagraphs:
            [
                "Se você está interessado em se aprofundar em Matemática para a Precisão, especialmente em Medidas e Escalas na Metalurgia, aqui estão algumas dicas para maximizar sua experiência online. Primeiro, crie um espaço de estudo livre de distrações, onde você possa se concentrar totalmente no material.",
                "Estabeleça uma rotina de estudos, reservando horários específicos para assistir às aulas e realizar as atividades. Além disso, interaja com seus colegas e instrutores nos fóruns e chats disponíveis, pois isso enriquece o aprendizado e ajuda a esclarecer dúvidas. Não se esqueça de revisar o conteúdo regularmente e aplicar os conhecimentos em situações práticas sempre que possível.",
                "Por fim, mantenha-se atualizado sobre as novas técnicas e normas em medidas e escalas na metalurgia, já que essa área está sempre evoluindo. Com dedicação e as estratégias corretas, você estará preparado para se destacar no setor!"
            ],
            recommendationParagraphs:
            [
                "Se você está interessado em se aprofundar na matemática aplicada à metalurgia, aqui estão algumas dicas valiosas. Primeiro, escolha um curso que ofereça uma abordagem prática, com aulas que incluam exercícios e estudos de caso. Isso ajudará a entender como aplicar medidas e escalas na prática. Além disso, verifique se o curso é reconhecido por instituições relevantes, pois isso pode impactar sua formação e carreira.",
                "Outro aspecto importante é a experiência dos instrutores. Procure cursos que tenham profissionais com experiência no setor, pois eles podem compartilhar insights valiosos que vão além da teoria.",
                "Considere também a carga horária e a flexibilidade do curso. Se você trabalha, um curso que ofereça aulas noturnas ou aos finais de semana pode ser mais conveniente.",
                "Por fim, não se esqueça de se conectar com outros alunos e profissionais da área. Isso pode abrir portas e proporcionar um aprendizado ainda mais enriquecedor. Investir em um bom curso de matemática para a metalurgia é essencial para garantir precisão e eficiência no trabalho."
            ]),
        2 => CreatePresentation(
            onlineIntroduction: "Os encontros presenciais de modelagem podem ser acompanhados por materiais digitais que apoiam a leitura de formas, medidas e estampas:",
            onlineTopics:
            [
                "Guias ilustrados para revisar formas geométricas e eixos de simetria em composições têxteis.",
                "Fichas de medidas e escalas para consulta durante exercícios de modelagem.",
                "Exemplos digitais de planificação e repetição de motivos em estampas.",
                "Acesso aos materiais de apoio por computador, tablet ou celular."
            ],
            onlineAccessibility: "Os materiais de apoio usam textos legíveis e descrições para orientar a consulta digital; eles complementam os encontros presenciais e não substituem a formação em sala.",
            assessmentIntroduction: "As atividades de avaliação relacionam conceitos geométricos às etapas de criação e produção têxtil:",
            assessmentTopics:
            [
                "Exercícios de identificação de formas e simetrias em padrões de tecidos.",
                "Atividades de cálculo e comparação de medidas e proporções.",
                "Análise de um molde planificado e de sua aplicação em uma estampa.",
                "Síntese final com justificativa das escolhas de escala e composição."
            ],
            assessmentConclusion: "Os resultados das atividades permitem revisar conceitos de geometria e modelagem; a prévia não registra notas ou conclusão de participantes.",
            faqs: CreateCourseFaqs("Geometria e Modelagem no Design Têxtil", "geometria, simetria e modelagem têxtil", "modelistas e profissionais de produção têxtil", "presencial", 32),
            articleParagraphs:
            [
                "A modelagem têxtil ganha clareza quando formas, medidas e proporções são registradas antes do corte. Ao estudar geometria aplicada, observe como cada linha e cada escala orientam a construção de uma peça ou composição.",
                "Durante os exercícios, compare os eixos de simetria e a repetição dos motivos. Anotar as medidas de referência ajuda a manter alinhamento entre o molde e a estampa, além de facilitar a revisão em equipe.",
                "Leve essas observações aos encontros presenciais e use os materiais digitais como apoio. Assim, conceitos geométricos podem ser relacionados às decisões cotidianas de design e produção têxtil."
            ],
            recommendationParagraphs:
            [
                "Separe amostras de estampas diferentes e identifique nelas as formas que se repetem.",
                "Registre as medidas utilizadas em cada exercício para comparar escalas e proporções.",
                "Revise a planificação antes de transferir o modelo para o tecido.",
                "Troque observações com a equipe de modelagem para conferir alinhamento e simetria."
            ]),
        3 => CreatePresentation(
            onlineIntroduction: "O conteúdo digital organiza conceitos estatísticos em exemplos ligados aos registros de qualidade e produtividade:",
            onlineTopics:
            [
                "Vídeos curtos para interpretar tabelas e gráficos de produção.",
                "Planilhas de exercício com conjuntos fictícios de medições.",
                "Estudos de caso sobre variação, amostragem e indicadores de processo.",
                "Materiais acessíveis em computador, tablet ou celular para revisão dos conceitos."
            ],
            onlineAccessibility: "Os materiais incluem texto estruturado e recursos de acessibilidade para apoiar a leitura de tabelas e gráficos.",
            assessmentIntroduction: "As avaliações usam conjuntos fictícios de dados para observar a compreensão de estatística aplicada:",
            assessmentTopics:
            [
                "Leitura de tabelas de conformidade e produtividade.",
                "Interpretação de gráficos de variação do processo.",
                "Escolha de uma amostra e síntese dos resultados observados.",
                "Proposta de indicador e ação de acompanhamento da qualidade."
            ],
            assessmentConclusion: "As atividades são demonstrativas e apoiam a interpretação de dados; não certificam uma operação industrial real nem guardam resultados de alunos.",
            faqs: CreateCourseFaqs("Estatística e Controle de Qualidade na Produção", "estatística, amostragem e controle de qualidade", "inspetores de qualidade e líderes de produção", "online", 24),
            articleParagraphs:
            [
                "Dados de produção ajudam a descrever o que acontece em um processo, mas precisam ser lidos com contexto. Antes de comparar resultados, confira unidades, período de coleta e critérios de qualidade utilizados.",
                "A amostragem permite observar um conjunto de itens sem confundir uma medição isolada com todo o lote. Gráficos e medidas-resumo tornam a variação mais visível e ajudam a formular perguntas para a equipe.",
                "Use os exemplos fictícios para praticar uma leitura cuidadosa: registre o que os dados mostram, o que ainda não permitem concluir e quais informações seriam úteis para acompanhar a produtividade."
            ],
            recommendationParagraphs:
            [
                "Confira a origem e a unidade de cada coluna antes de interpretar uma tabela.",
                "Compare períodos equivalentes para evitar conclusões baseadas em recortes diferentes.",
                "Use gráficos para observar tendências, sem ignorar os valores individuais relevantes.",
                "Apresente os resultados com clareza e discuta as ações com as equipes responsáveis."
            ]),
        4 => CreatePresentation(
            onlineIntroduction: "Os materiais digitais apoiam a revisão dos conceitos físicos trabalhados em situações de operação e manutenção presencial:",
            onlineTopics:
            [
                "Animações explicativas sobre deslocamento e movimento de máquinas.",
                "Guias de consulta sobre força, atrito e trabalho mecânico.",
                "Diagramas de transferência de calor em fornos e componentes.",
                "Materiais de apoio para discutir eficiência energética com a equipe."
            ],
            onlineAccessibility: "Diagramas e textos de apoio são organizados para consulta acessível e complementam os encontros presenciais sobre equipamentos.",
            assessmentIntroduction: "As atividades relacionam grandezas físicas a exemplos fictícios de máquinas, fornos e prensas:",
            assessmentTopics:
            [
                "Descrição de trajetórias, deslocamentos e velocidades de operação.",
                "Análise de forças e atrito entre componentes.",
                "Comparação de trabalho, potência e consumo de energia.",
                "Identificação de formas de transferência de calor e possíveis perdas."
            ],
            assessmentConclusion: "A avaliação consolida a interpretação de conceitos físicos; os exemplos não substituem procedimentos técnicos nem normas de segurança do local de trabalho.",
            faqs: CreateCourseFaqs("Física do Movimento e da Energia na Metalurgia", "movimento, forças, calor e energia na metalurgia", "operadores e técnicos de manutenção industrial", "presencial", 36),
            articleParagraphs:
            [
                "Observar o movimento de uma máquina é um ponto de partida para relacionar deslocamento, tempo e velocidade. Registre as condições da operação e considere que mudanças de carga podem alterar o comportamento do equipamento.",
                "Forças e atrito também influenciam a interação entre peças e superfícies. Ao analisar um exemplo, identifique os componentes envolvidos e diferencie uma descrição física de uma instrução operacional.",
                "Calor e energia aparecem em diferentes etapas da metalurgia, especialmente em fornos e processos de transformação. Use os conceitos como apoio para conversar sobre eficiência e manutenção com a equipe técnica."
            ],
            recommendationParagraphs:
            [
                "Desenhe um esquema simples do equipamento antes de identificar as forças envolvidas.",
                "Anote as unidades usadas ao comparar velocidade, potência ou temperatura.",
                "Relacione os exemplos dos materiais aos procedimentos e orientações locais de segurança.",
                "Discuta oportunidades de eficiência sem alterar práticas operacionais autorizadas."
            ]),
        5 => CreatePresentation(
            onlineIntroduction: "As aulas digitais relacionam conceitos de química às etapas de tinturaria e acabamento de tecidos:",
            onlineTopics:
            [
                "Vídeos explicativos sobre reações envolvidas em processos de tingimento.",
                "Esquemas de parâmetros de processo e uso de insumos.",
                "Estudos fictícios sobre tratamento de efluentes e resíduos.",
                "Materiais de revisão acessíveis em diferentes dispositivos."
            ],
            onlineAccessibility: "Textos e esquemas apresentam os conceitos de modo estruturado e contam com recursos de acessibilidade para leitura digital.",
            assessmentIntroduction: "As avaliações demonstrativas exploram decisões relacionadas a processos químicos têxteis e sustentabilidade:",
            assessmentTopics:
            [
                "Identificação de etapas gerais de tinturaria e acabamento.",
                "Leitura de parâmetros e consumo de recursos em um processo fictício.",
                "Análise de alternativas de tratamento de efluentes.",
                "Síntese de medidas para reduzir resíduos e melhorar o uso de insumos."
            ],
            assessmentConclusion: "Os exercícios apoiam a compreensão de processos e sustentabilidade; não constituem orientação para manipular substâncias ou operar equipamentos.",
            faqs: CreateCourseFaqs("Química e Sustentabilidade", "reações químicas, tinturaria e sustentabilidade têxtil", "profissionais de tinturaria e gestão ambiental", "online", 20),
            articleParagraphs:
            [
                "A química está presente nas etapas de tinturaria e acabamento, desde a preparação dos materiais até a obtenção das características desejadas no tecido. Compreender cada etapa ajuda a interpretar parâmetros e registros do processo.",
                "O uso de água e insumos deve ser observado junto com a geração de efluentes e resíduos. Estudos de caso permitem comparar possibilidades de tratamento e discutir como decisões de processo se relacionam com a sustentabilidade.",
                "Use os exemplos do curso para organizar perguntas sobre recursos, segurança e destinação adequada. A aplicação prática deve sempre seguir procedimentos, fichas e orientações técnicas autorizadas."
            ],
            recommendationParagraphs:
            [
                "Revise os nomes e a finalidade das etapas antes de comparar processos de tinturaria.",
                "Observe os indicadores de consumo em relação ao volume de produção correspondente.",
                "Consulte os procedimentos e as fichas de segurança aplicáveis no ambiente de trabalho.",
                "Registre ideias de redução de resíduos para avaliação pelas equipes responsáveis."
            ]),
        6 => CreatePresentation(
            onlineIntroduction: "Os materiais digitais complementam os encontros presenciais sobre propriedades e seleção de materiais metálicos:",
            onlineTopics:
            [
                "Ilustrações para revisar a organização interna dos metais.",
                "Fichas comparativas de resistência, dureza e ductilidade.",
                "Guias introdutórios sobre tratamentos e ensaios mecânicos.",
                "Conteúdo de apoio para consulta em diferentes dispositivos."
            ],
            onlineAccessibility: "Os recursos digitais usam textos estruturados e descrições de apoio; eles complementam a análise presencial de materiais.",
            assessmentIntroduction: "As atividades de avaliação relacionam propriedades a escolhas e inspeções de materiais em exemplos industriais:",
            assessmentTopics:
            [
                "Identificação de conceitos básicos de estrutura dos metais.",
                "Comparação de resistência, dureza e ductilidade.",
                "Análise de objetivos gerais de tratamentos térmicos.",
                "Organização de critérios e registros de inspeção."
            ],
            assessmentConclusion: "A prévia verifica a compreensão de conceitos em situações fictícias e não substitui ensaios, laudos ou especificações técnicas.",
            faqs: CreateCourseFaqs("Ciência dos Materiais na Metalurgia", "estrutura, resistência e propriedades dos metais", "técnicos metalúrgicos e inspetores de materiais", "presencial", 30),
            articleParagraphs:
            [
                "A estrutura de um material influencia propriedades que importam na sua aplicação. Ao estudar metais, observe como organização interna, composição e processamento ajudam a explicar diferenças entre materiais.",
                "Resistência, dureza e ductilidade descrevem aspectos distintos do comportamento mecânico. Comparar essas propriedades exige considerar o tipo de solicitação e os requisitos da aplicação industrial.",
                "Tratamentos e inspeções devem ser entendidos a partir de critérios técnicos documentados. Os exemplos do curso apoiam a leitura desses critérios, sem substituir ensaios ou especificações do produto."
            ],
            recommendationParagraphs:
            [
                "Registre as propriedades de interesse antes de comparar materiais para uma aplicação.",
                "Diferencie dureza, resistência e ductilidade ao discutir resultados de ensaios.",
                "Consulte especificações técnicas e procedimentos autorizados ao analisar materiais reais.",
                "Mantenha registros de inspeção organizados para apoiar a rastreabilidade."
            ]),
        _ => throw new ArgumentOutOfRangeException(nameof(courseId), courseId, "Unknown course seed ID.")
    };

    private static CoursePresentation CreatePresentation(string onlineIntroduction, string[] onlineTopics,
        string onlineAccessibility, string assessmentIntroduction,
        string[] assessmentTopics, string assessmentConclusion, List<CourseFaq> faqs,
        string[] articleParagraphs, string[] recommendationParagraphs) => new()
    {
        Certification = "SESI",
        DetailsImagePath = "/assets/video-thumb.png",
        OnlineLessons = new CourseTextSection
        {
            Introduction = onlineIntroduction,
            Topics = [.. onlineTopics],
            Accessibility = onlineAccessibility
        },
        Assessments = new CourseTextSectionWithConclusion
        {
            Introduction = assessmentIntroduction,
            Topics = [.. assessmentTopics],
            Conclusion = assessmentConclusion
        },
        Faqs = faqs,
        LearningPreview = new LearningPreview
        {
            Category = "CERTIFICAÇÃO",
            ProgressPercent = 10,
            SearchLabel = "Buscar conteúdo do curso",
            BannerImagePath = "/assets/banner-ava.png",
            Introduction = new LearningIntroduction
            {
                Label = "1. Introdução",
                Expanded = true,
                Items =
                [
                    new LearningItem { Label = "1.1 Introdução", State = "completed" },
                    new LearningItem { Label = "1.2 Boas práticas", State = "current" },
                    new LearningItem { Label = "1.3 Extras", State = "pending" }
                ]
            },
            Modules = CreateGenericLearningModules(),
            Article = new LearningArticle
            {
                Title = "Boas práticas",
                Paragraphs = [.. articleParagraphs]
            },
            Recommendations = new LearningArticle
            {
                Title = "Recomendações",
                Paragraphs = [.. recommendationParagraphs]
            }
        }
    };

    private static List<LearningModule> CreateGenericLearningModules() =>
        Enumerable.Range(1, 6).Select(index =>
        {
            var moduleNumber = index + 1;
            return new LearningModule
            {
                Label = $"{moduleNumber}. Módulo {index}",
                Expanded = false,
                Lessons =
                [
                    new LearningLesson
                    {
                        Label = $"{moduleNumber}.1 Introdução ao módulo",
                        Description = "Apresentação demonstrativa dos objetivos e conceitos principais do módulo."
                    },
                    new LearningLesson
                    {
                        Label = $"{moduleNumber}.2 Revisão dos conceitos",
                        Description = "Revisão demonstrativa do conteúdo antes de avançar para o próximo módulo."
                    }
                ]
            };
        }).ToList();

    private static List<CourseFaq> CreateMathematicsFaqs() =>
    [
        Faq("O curso é totalmente online?", "Sim. Todas as aulas, atividades e avaliações são realizadas pela plataforma digital, permitindo que o aluno estude de onde estiver, no seu próprio ritmo."),
        Faq("Preciso estar disponível em horários específicos?", "Não. As aulas são gravadas e ficam disponíveis 24 horas por dia. O aluno pode acessar o conteúdo quando for mais conveniente, respeitando os prazos de cada módulo."),
        Faq("Como são feitas as avaliações?", "As avaliações são realizadas diretamente na plataforma, por meio de provas objetivas, estudos de caso e atividades práticas. Algumas avaliações podem ter tempo limitado e exigem atenção às instruções."),
        Faq("O curso oferece certificado?", "Sim. Ao concluir todos os módulos e ser aprovado nas avaliações, o aluno recebe um certificado digital de conclusão, válido em todo o território nacional."),
        Faq("Quem são os professores?", "O corpo docente é formado por especialistas, engenheiros e professores com ampla experiência em segurança do trabalho e atuação direta na indústria. Os perfis dos docentes estão disponíveis na plataforma."),
        Faq("Há suporte para dúvidas durante o curso?", "Sim. Os alunos podem enviar dúvidas por meio dos fóruns de discussão, chats da plataforma ou durante os webinários ao vivo. A equipe pedagógica está disponível para auxiliar durante todo o curso."),
        Faq("O curso exige algum conhecimento prévio?", "Não é obrigatório, mas é recomendado que o aluno tenha interesse ou alguma familiaridade com o ambiente industrial. O curso foi estruturado para atender tanto iniciantes quanto profissionais em busca de atualização."),
        Faq("Posso acessar o curso pelo celular?", "Sim. A plataforma é responsiva e compatível com computadores, tablets e smartphones, garantindo uma experiência completa em qualquer dispositivo."),
        Faq("Quanto tempo tenho para concluir o curso?", "O prazo para conclusão varia conforme o plano escolhido. Em geral, o aluno tem entre 3 a 6 meses para finalizar todos os módulos e avaliações."),
        Faq("O curso é reconhecido por órgãos oficiais?", "O curso segue as diretrizes das Normas Regulamentadoras (NRs) e boas práticas da área, sendo reconhecido como formação complementar. Para certificações oficiais, é necessário verificar os requisitos específicos de cada órgão regulador.")
    ];

    private static List<CourseFaq> CreateCourseFaqs(string courseName, string theme, string audience,
        string modality, int workloadHours) =>
    [
        Faq($"Quais temas de {theme} são abordados?", $"A apresentação de {courseName} percorre {theme} com exemplos fictícios ligados aos processos industriais."),
        Faq("Para quem esta formação é indicada?", $"O conteúdo foi organizado especialmente para {audience}, sem impedir que outras pessoas interessadas acompanhem os conceitos introdutórios."),
        Faq("Preciso ter experiência prévia?", $"Não é exigida experiência prévia. Os módulos de {courseName} introduzem os conceitos e avançam para situações relacionadas a {theme}."),
        Faq($"A formação é {modality}?", modality == "presencial"
            ? "Sim. Os encontros da formação são presenciais; os materiais digitais servem como apoio e não tornam toda a formação online."
            : "Sim. O conteúdo é apresentado online e pode ser consultado em diferentes dispositivos, conforme a organização demonstrativa da formação."),
        Faq("Como o conteúdo se relaciona à indústria?", $"Os exemplos de {courseName} relacionam {theme} a situações fictícias de produção e trabalho técnico."),
        Faq("Como são as atividades de aprendizagem?", "A prévia reúne leituras, exercícios e estudos de caso para revisar os conceitos sem executar operações em equipamentos reais."),
        Faq("Quais materiais podem apoiar os estudos?", "Textos, esquemas e exemplos de aplicação acompanham os tópicos e ajudam a revisar o conteúdo apresentado."),
        Faq("Qual é a carga horária informada?", $"A carga horária fictícia deste curso é de {workloadHours} horas; ela não representa calendário ou turma em andamento."),
        Faq("Como são apresentadas as avaliações?", "As avaliações demonstrativas usam perguntas e situações fictícias para apoiar a revisão dos módulos, sem registrar notas ou progresso de participantes."),
        Faq("A prévia confirma uma certificação profissional?", "A apresentação informa certificação SESI como conteúdo editorial; a prévia não realiza matrícula nem substitui a consulta a requisitos oficiais de certificação.")
    ];

    private static CourseFaq Faq(string question, string answer) => new() { Question = question, Answer = answer };

    private static string CardImagePath(int courseId) => courseId switch
    {
        1 or 6 => "/assets/banner-ava.png",
        2 => "/assets/hero-image.png",
        _ => "/assets/video-thumb.png"
    };

    private static Course BuildCourse(int id, string fullName, string shortName, string shortDescription,
        string description,
        int categoryId, int modalityId, int statusId, decimal workloadHours, string targetAudience,
        DateOnly? availableOn, CourseModule[] modules) => new()
    {
        Id = id, FullName = fullName, ShortName = shortName, Description = description,
        ShortDescription = shortDescription, PresentationJson = CoursePresentationSerializer.Serialize(BuildPresentation(id)),
        CategoryId = categoryId, ModalityId = modalityId, CourseStatusId = statusId,
        WorkloadHours = workloadHours, TargetAudience = targetAudience, AvailableOn = availableOn,
        ImageUrl = CardImagePath(id), Modules = modules
    };

    private static CourseModule BuildModule(int courseId, int order, string title, string description,
        params (string Title, string Description)[] lessons)
    {
        var moduleId = courseId * 100 + order;
        return new CourseModule
        {
            Id = moduleId, CourseId = courseId, Title = title, Description = description, DisplayOrder = order,
            Lessons = lessons.Select((lesson, index) => new Lesson
            {
                Id = moduleId * 100 + index + 1, CourseModuleId = moduleId,
                Title = lesson.Title, Description = lesson.Description, DisplayOrder = index + 1
            }).ToList()
        };
    }
}
