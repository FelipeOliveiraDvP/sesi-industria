using Catalog.Api.Entities;

namespace Catalog.Api.Data.Seed;

public static class CatalogSeedData
{
    public static Category[] CreateCategories() =>
    [
        new() { Id = 1, Name = "Tecnologia Educacional" },
        new() { Id = 2, Name = "Gestão e Projetos" },
        new() { Id = 3, Name = "Segurança e Bem-estar" }
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
            id: 1, fullName: "Fundamentos de Tecnologia Educacional", shortName: "TecEdu",
            description: "Exploração de recursos digitais para organizar materiais e planejar atividades educacionais em situações fictícias.",
            categoryId: 1, modalityId: 1, statusId: 1, workloadHours: 12,
            targetAudience: "Educadores", availableOn: null,
            modules:
            [
                BuildModule(1, 1, "Recursos para ensinar", "Seleção e organização de materiais para encontros de aprendizagem.",
                    ("Materiais digitais", "Comparação de textos, imagens e áudios em um acervo educacional fictício."),
                    ("Planejamento de atividades", "Construção de uma sequência de atividades usando recursos digitais simples."))
            ]),
        BuildCourse(
            id: 2, fullName: "Oficina de Aprendizagem Digital", shortName: "DigitalLab",
            description: "Experimentação de atividades participativas e adaptação de materiais digitais para diferentes contextos de aprendizagem.",
            categoryId: 1, modalityId: 2, statusId: 2, workloadHours: 20,
            targetAudience: "Instrutores", availableOn: new DateOnly(2027, 2, 10),
            modules:
            [
                BuildModule(2, 1, "Atividades interativas", "Desenvolvimento de propostas práticas para encontros formativos.",
                    ("Roteiro de oficina", "Organização das etapas de uma oficina sobre um tema fictício."),
                    ("Dinâmicas em grupo", "Experimentação de tarefas colaborativas com diferentes formatos de participação.")),
                BuildModule(2, 2, "Acessibilidade de materiais", "Adaptação de conteúdos para diferentes formas de acesso.",
                    ("Legibilidade", "Revisão de contraste, tamanho e organização de textos de exemplo."),
                    ("Descrição de imagens", "Escrita de descrições alternativas para imagens educacionais fictícias."),
                    ("Revisão da atividade", "Verificação de um roteiro adaptado a diferentes necessidades de aprendizagem."))
            ]),
        BuildCourse(
            id: 3, fullName: "Projetos Colaborativos para Educação", shortName: "Projetos",
            description: "Planejamento e organização de projetos educacionais colaborativos com exemplos de equipes e resultados fictícios.",
            categoryId: 2, modalityId: 1, statusId: 1, workloadHours: 24,
            targetAudience: "Coordenadores", availableOn: new DateOnly(2026, 3, 1),
            modules:
            [
                BuildModule(3, 1, "Desenho do projeto", "Definição de objetivos e atividades de um projeto fictício.",
                    ("Problema e objetivo", "Delimitação de uma situação educacional e dos resultados pretendidos."),
                    ("Plano de atividades", "Distribuição de etapas e recursos de um projeto colaborativo de exemplo.")),
                BuildModule(3, 2, "Organização do trabalho", "Coordenação de responsabilidades de uma equipe fictícia.",
                    ("Acordos da equipe", "Construção de combinados para comunicação e divisão de responsabilidades.")),
                BuildModule(3, 3, "Apresentação dos resultados", "Preparação dos registros e da comunicação de resultados.",
                    ("Registros do projeto", "Seleção de evidências e materiais produzidos em uma experiência fictícia."),
                    ("Síntese colaborativa", "Elaboração de uma apresentação de aprendizados e resultados do projeto."))
            ]),
        BuildCourse(
            id: 4, fullName: "Comunicação em Equipes Educacionais", shortName: "Conecta",
            description: "Práticas de diálogo e organização de mensagens em situações fictícias de trabalho de equipes educacionais.",
            categoryId: 2, modalityId: 2, statusId: 3, workloadHours: 8,
            targetAudience: "Lideranças educacionais", availableOn: new DateOnly(2025, 8, 20),
            modules:
            [
                BuildModule(4, 1, "Diálogo na equipe", "Exercícios de escuta e comunicação em situações cotidianas.",
                    ("Escuta ativa", "Identificação de necessidades e pontos de vista em diálogos de personagens fictícios."),
                    ("Mensagens objetivas", "Reescrita de comunicados de exemplo para tornar sua finalidade clara."),
                    ("Conversa colaborativa", "Organização de uma conversa para construir acordos entre integrantes de uma equipe."))
            ]),
        BuildCourse(
            id: 5, fullName: "Segurança nas Rotinas de Aprendizagem", shortName: "AprenderSeguro",
            description: "Observação de ambientes educacionais e organização de cuidados preventivos em cenários fictícios de aprendizagem.",
            categoryId: 3, modalityId: 1, statusId: 2, workloadHours: 16,
            targetAudience: "Comunidade escolar", availableOn: new DateOnly(2027, 3, 5),
            modules:
            [
                BuildModule(5, 1, "Percepção de riscos", "Leitura de situações de exemplo em ambientes de aprendizagem.",
                    ("Observação do ambiente", "Identificação de situações de atenção em uma sala de atividades fictícia."),
                    ("Organização de espaços", "Discussão de possibilidades de circulação e disposição de materiais.")),
                BuildModule(5, 2, "Rotinas preventivas", "Planejamento de cuidados cotidianos para atividades educacionais.",
                    ("Preparação de atividades", "Elaboração de uma lista de cuidados para uma atividade prática fictícia."),
                    ("Comunicação de cuidados", "Construção de orientações claras para participantes de um encontro educacional."))
            ]),
        BuildCourse(
            id: 6, fullName: "Bem-estar em Ambientes Educacionais", shortName: "Cuidar",
            description: "Reflexão sobre hábitos de cuidado e convivência em ambientes educacionais com exemplos e personagens fictícios.",
            categoryId: 3, modalityId: 2, statusId: 3, workloadHours: 6,
            targetAudience: "Equipes de apoio", availableOn: null,
            modules:
            [
                BuildModule(6, 1, "Hábitos de cuidado", "Organização de pequenas práticas de cuidado ao longo da rotina.",
                    ("Pausas na rotina", "Análise de uma jornada fictícia para reconhecer oportunidades de pausa e organização.")),
                BuildModule(6, 2, "Ambientes acolhedores", "Práticas de convivência e acolhimento em espaços educacionais.",
                    ("Acolhimento cotidiano", "Discussão de abordagens para receber diferentes participantes em uma atividade."),
                    ("Combinados de convivência", "Elaboração coletiva de combinados em um grupo educacional fictício."))
            ])
    ];

    private static Course BuildCourse(int id, string fullName, string shortName, string description,
        int categoryId, int modalityId, int statusId, decimal workloadHours, string targetAudience,
        DateOnly? availableOn, CourseModule[] modules) => new()
    {
        Id = id, FullName = fullName, ShortName = shortName, Description = description,
        CategoryId = categoryId, ModalityId = modalityId, CourseStatusId = statusId,
        WorkloadHours = workloadHours, TargetAudience = targetAudience, AvailableOn = availableOn,
        ImageUrl = $"/images/courses/course-{id}.svg", Modules = modules
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
