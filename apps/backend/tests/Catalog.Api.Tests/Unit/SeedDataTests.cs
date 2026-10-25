using System.Text.Json;
using Catalog.Api.Content;
using Catalog.Api.Data.Seed;
using Xunit;

namespace Catalog.Api.Tests.Unit;

[Trait("Category", "Unit")]
public sealed class SeedDataTests
{
    [Fact]
    public void MatchesTheSixLandingCourseTitlesAndBriefDescriptions()
    {
        var expected = new (int Id, string FullName, string ShortName, string ShortDescription)[]
        {
            (1, "Matemática para a Precisão: Medidas e Escalas na Metalurgia", "Metrologia",
                "Aplicação de grandezas, proporções e escalas em processos de corte, furação e usinagem."),
            (2, "Geometria e Modelagem no Design Têxtil", "GeoTêxtil",
                "Explora formas geométricas, simetria e padrões matemáticos presentes em tecidos e estampas."),
            (3, "Estatística e Controle de Qualidade na Produção", "QualiDados",
                "Leitura e interpretação de dados de controle de qualidade e produtividade."),
            (4, "Física do Movimento e da Energia na Metalurgia", "Física Industrial",
                "Estudo de forças, atrito, calor e energia em máquinas, fornos e prensas."),
            (5, "Química e Sustentabilidade", "Química Têxtil",
                "Estudo das reações químicas envolvidas na tinturaria e acabamento de tecidos."),
            (6, "Ciência dos Materiais na Metalurgia", "Materiais Industriais",
                "Conceitos básicos de estrutura, resistência e propriedades dos metais.")
        };
        var actual = CatalogSeedData.CreateCourses()
            .Select(course => (course.Id, course.FullName, course.ShortName, course.ShortDescription)).ToArray();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ProvidesRequiredClassificationsWithoutAddingACourseForLanguages()
    {
        var courses = CatalogSeedData.CreateCourses();
        var categories = CatalogSeedData.CreateCategories();
        var modalities = CatalogSeedData.CreateModalities();
        var statuses = CatalogSeedData.CreateStatuses();

        Assert.Equal(6, courses.Length);
        Assert.Equal(new[] { "Matemática", "Ciências da Natureza", "Linguagens" },
            categories.Select(category => category.Name));
        Assert.Equal(new[] { "Online", "Presencial" }, modalities.Select(modality => modality.Name));
        Assert.Equal(new[] { "Disponível", "Em breve", "Encerrado" }, statuses.Select(status => status.Name));
        Assert.Equal(new[] { 1, 1, 1, 2, 2, 2 }, courses.Select(course => course.CategoryId));
        Assert.DoesNotContain(courses, course => course.CategoryId == 3);
        Assert.All(courses, course =>
        {
            Assert.Contains(categories, category => category.Id == course.CategoryId);
            Assert.Contains(modalities, modality => modality.Id == course.ModalityId);
            Assert.Contains(statuses, status => status.Id == course.CourseStatusId);
        });
    }

    [Fact]
    public void CreatesTheDocumentedOrderedCourseStructuresWithStableRelationships()
    {
        var courses = CatalogSeedData.CreateCourses();
        var math = Assert.Single(courses, course => course.Id == 1);
        var expectedMath = new (string Title, string[] Topics)[]
        {
            ("Introdução à Matemática na Metalurgia", ["Conceitos fundamentais de medidas e escalas aplicados à metalurgia.", "Importância da precisão nas medições e controle de qualidade.", "Panorama das normas e regulamentos no Brasil relacionados à metalurgia."]),
            ("Identificação e Avaliação de Medidas", ["Tipos de medidas utilizadas: comprimento, massa, volume e temperatura.", "Técnicas de análise de precisão nas medições.", "Elaboração de gráficos e tabelas de medidas."]),
            ("Normas e Regulamentações", ["Estudo das principais normas aplicáveis à metalurgia (ABNT, ISO, entre outras).", "Interpretação e aplicação prática das normas de medição.", "Atualizações e impactos legais nas práticas de medição."]),
            ("Instrumentos de Medição", ["Tipos de instrumentos utilizados na metalurgia: paquímetros, micrômetros e balanças.", "Critérios de seleção, uso e calibração.", "Responsabilidades legais de operadores e supervisores."]),
            ("Prevenção de Erros de Medição", ["Tipos de erros e incertezas nas medições.", "Procedimentos para garantir a precisão e exatidão.", "Treinamento prático em técnicas de medição."]),
            ("Ergonomia e Eficiência na Medição", ["Princípios ergonômicos aplicados ao uso de instrumentos de medição.", "Prevenção de lesões por uso inadequado de ferramentas.", "Qualidade de vida no ambiente de trabalho."]),
            ("Análise de Resultados de Medição", ["Técnicas de análise de dados de medições.", "Elaboração de relatórios e planos de ação para melhorias.", "Indicadores de precisão e melhoria contínua."]),
            ("Cultura de Precisão e Comportamento Seguro", ["Desenvolvimento de uma cultura organizacional voltada à precisão nas medições.", "Comunicação eficaz e liderança em práticas de medição.", "Programas de conscientização e engajamento dos colaboradores."]),
            ("Tecnologias Aplicadas à Medição", ["Uso de plataformas digitais, sensores e automação na medição de precisão.", "Sistemas de gestão de qualidade.", "Tendências e inovações no setor metalúrgico."]),
            ("Avaliação Final e Certificação", ["Teste de conhecimentos adquiridos sobre medidas e escalas.", "Atividades práticas e estudos de caso.", "Emissão de certificado digital de conclusão."])
        };
        Assert.Equal(10, math.Modules.Count);
        Assert.Equal(30, math.Modules.Sum(module => module.Lessons.Count));
        Assert.Equal(expectedMath.Select(module => module.Title), math.Modules.OrderBy(module => module.DisplayOrder).Select(module => module.Title));
        Assert.Equal(expectedMath.SelectMany(module => module.Topics), math.Modules.OrderBy(module => module.DisplayOrder)
            .SelectMany(module => module.Lessons.OrderBy(lesson => lesson.DisplayOrder).Select(lesson => lesson.Description)));

        var expectedCounts = new (int CourseId, string[] ModuleTitles, int[] LessonCounts)[]
        {
            (2, ["Formas e simetrias", "Medidas e proporções", "Modelagem e planificação", "Aplicação em estampas"], [2, 3, 2, 2]),
            (3, ["Leitura de dados", "Variação e amostragem", "Controle de processos"], [2, 3, 2]),
            (4, ["Movimento", "Forças e atrito", "Trabalho e potência", "Calor e transferência", "Eficiência energética"], [2, 2, 3, 2, 2]),
            (5, ["Reações e processos têxteis", "Uso de recursos e tratamento de resíduos"], [3, 3]),
            (6, ["Estrutura dos metais", "Propriedades mecânicas", "Tratamentos e seleção", "Inspeção de materiais"], [2, 3, 2, 2])
        };
        foreach (var (courseId, titles, lessonCounts) in expectedCounts)
        {
            var course = Assert.Single(courses, candidate => candidate.Id == courseId);
            var modules = course.Modules.OrderBy(module => module.DisplayOrder).ToArray();
            Assert.Equal(titles, modules.Select(module => module.Title));
            Assert.Equal(lessonCounts, modules.Select(module => module.Lessons.Count));
        }

        var allModules = courses.SelectMany(course => course.Modules).ToArray();
        var allLessons = allModules.SelectMany(module => module.Lessons).ToArray();
        Assert.Equal(allModules.Length, allModules.Select(module => module.Id).Distinct().Count());
        Assert.Equal(allLessons.Length, allLessons.Select(lesson => lesson.Id).Distinct().Count());
        Assert.All(courses, course => Assert.All(course.Modules, module =>
        {
            Assert.Equal(course.Id, module.CourseId);
            Assert.False(string.IsNullOrWhiteSpace(module.Description));
            Assert.Equal(Enumerable.Range(1, module.Lessons.Count), module.Lessons.OrderBy(lesson => lesson.DisplayOrder)
                .Select(lesson => lesson.DisplayOrder));
            Assert.All(module.Lessons, lesson =>
            {
                Assert.Equal(module.Id, lesson.CourseModuleId);
                Assert.False(string.IsNullOrWhiteSpace(lesson.Title));
                Assert.False(string.IsNullOrWhiteSpace(lesson.Description));
            });
        }));
    }

    [Fact]
    public void ProvidesCompleteCourseSpecificDetailsAndLearningPresentations()
    {
        var courses = CatalogSeedData.CreateCourses();
        var presentations = courses.ToDictionary(course => course.Id,
            course => CoursePresentationSerializer.Deserialize(course.PresentationJson));

        Assert.All(presentations.Values, presentation =>
        {
            Assert.Equal(10, presentation.Faqs.Count);
            Assert.Equal(10, presentation.LearningPreview.ProgressPercent);
            Assert.Equal(3, presentation.LearningPreview.Article.Paragraphs.Count);
            Assert.Equal(4, presentation.LearningPreview.Recommendations.Paragraphs.Count);
            Assert.Equal(3, presentation.LearningPreview.Introduction.Items.Count);
            Assert.Equal(6, presentation.LearningPreview.Modules.Count);
            Assert.All(presentation.LearningPreview.Modules, module => Assert.Equal(2, module.Lessons.Count));
        });

        var genericAvaOutline = JsonSerializer.Serialize(presentations[1].LearningPreview.Modules);
        Assert.All(presentations.Values, presentation =>
            Assert.Equal(genericAvaOutline, JsonSerializer.Serialize(presentation.LearningPreview.Modules)));
        Assert.NotEqual(
            courses.Single(course => course.Id == 1).Modules.OrderBy(module => module.DisplayOrder).First().Title,
            courses.Single(course => course.Id == 2).Modules.OrderBy(module => module.DisplayOrder).First().Title);

        var math = presentations[1];
        Assert.Equal(
        [
            "Se você está interessado em se aprofundar em Matemática para a Precisão, especialmente em Medidas e Escalas na Metalurgia, aqui estão algumas dicas para maximizar sua experiência online. Primeiro, crie um espaço de estudo livre de distrações, onde você possa se concentrar totalmente no material.",
            "Estabeleça uma rotina de estudos, reservando horários específicos para assistir às aulas e realizar as atividades. Além disso, interaja com seus colegas e instrutores nos fóruns e chats disponíveis, pois isso enriquece o aprendizado e ajuda a esclarecer dúvidas. Não se esqueça de revisar o conteúdo regularmente e aplicar os conhecimentos em situações práticas sempre que possível.",
            "Por fim, mantenha-se atualizado sobre as novas técnicas e normas em medidas e escalas na metalurgia, já que essa área está sempre evoluindo. Com dedicação e as estratégias corretas, você estará preparado para se destacar no setor!"
        ], math.LearningPreview.Article.Paragraphs);
        Assert.Equal(
        [
            "Se você está interessado em se aprofundar na matemática aplicada à metalurgia, aqui estão algumas dicas valiosas. Primeiro, escolha um curso que ofereça uma abordagem prática, com aulas que incluam exercícios e estudos de caso. Isso ajudará a entender como aplicar medidas e escalas na prática. Além disso, verifique se o curso é reconhecido por instituições relevantes, pois isso pode impactar sua formação e carreira.",
            "Outro aspecto importante é a experiência dos instrutores. Procure cursos que tenham profissionais com experiência no setor, pois eles podem compartilhar insights valiosos que vão além da teoria.",
            "Considere também a carga horária e a flexibilidade do curso. Se você trabalha, um curso que ofereça aulas noturnas ou aos finais de semana pode ser mais conveniente.",
            "Por fim, não se esqueça de se conectar com outros alunos e profissionais da área. Isso pode abrir portas e proporcionar um aprendizado ainda mais enriquecedor. Investir em um bom curso de matemática para a metalurgia é essencial para garantir precisão e eficiência no trabalho."
        ], math.LearningPreview.Recommendations.Paragraphs);
        Assert.Equal("O curso é totalmente online?", math.Faqs[0].Question);
        Assert.Equal("O curso é reconhecido por órgãos oficiais?", math.Faqs[^1].Question);
        Assert.Equal(new[] { "completed", "current", "pending" }, math.LearningPreview.Introduction.Items.Select(item => item.State));

        var articleBodies = presentations.Values.Select(presentation =>
            string.Join(" ", presentation.LearningPreview.Article.Paragraphs)).ToArray();
        Assert.Equal(6, articleBodies.Distinct().Count());
        foreach (var course in courses.Where(course => course.ModalityId == 2))
        {
            var presentation = presentations[course.Id];
            Assert.Contains("presencia", presentation.OnlineLessons.Introduction, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("todas as aulas são online", presentation.OnlineLessons.Introduction, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void UsesTheDocumentedLocalFrontendAssetPaths()
    {
        var courses = CatalogSeedData.CreateCourses();
        Assert.Equal(
        [
            "/assets/banner-ava.png",
            "/assets/hero-image.png",
            "/assets/video-thumb.png",
            "/assets/video-thumb.png",
            "/assets/video-thumb.png",
            "/assets/banner-ava.png"
        ], courses.OrderBy(course => course.Id).Select(course => course.ImageUrl));

        foreach (var course in courses)
        {
            var presentation = CoursePresentationSerializer.Deserialize(course.PresentationJson);
            Assert.Equal("/assets/video-thumb.png", presentation.DetailsImagePath);
            Assert.Equal("/assets/banner-ava.png", presentation.LearningPreview.BannerImagePath);
            Assert.StartsWith("/assets/", course.ImageUrl);
        }
    }

    [Fact]
    public void DefinesMinimumFictionalCatalogAndValidClassifications()
    {
        var courses = CatalogSeedData.CreateCourses();
        var categories = CatalogSeedData.CreateCategories();
        var modalities = CatalogSeedData.CreateModalities();
        var statuses = CatalogSeedData.CreateStatuses();
        Assert.True(courses.Length >= 6);
        Assert.True(categories.Select(category => category.Name).Distinct().Count() >= 3);
        Assert.True(modalities.Select(modality => modality.Name).Distinct().Count() >= 2);
        Assert.True(statuses.Select(status => status.Name).Distinct().Count() >= 3);
        Assert.Equal(courses.Length, courses.Select(course => course.Id).Distinct().Count());

        Assert.All(courses, course =>
        {
            Assert.All(new[] { course.FullName, course.ShortName, course.Description, course.TargetAudience, course.ImageUrl },
                value => Assert.False(string.IsNullOrWhiteSpace(value)));
            Assert.Contains(categories, category => category.Id == course.CategoryId);
            Assert.Contains(modalities, modality => modality.Id == course.ModalityId);
            Assert.Contains(statuses, status => status.Id == course.CourseStatusId);
            Assert.StartsWith("/assets/", course.ImageUrl);
        });
    }

    [Fact]
    public void VariesWorkloadsAudiencesModulesLessonsAndAvailability()
    {
        var courses = CatalogSeedData.CreateCourses();
        Assert.True(courses.Select(course => course.WorkloadHours).Distinct().Count() > 1);
        Assert.True(courses.Select(course => course.TargetAudience).Distinct().Count() > 1);
        Assert.True(courses.Select(course => course.Modules.Count).Distinct().Count() > 1);
        Assert.Contains(courses, course => course.AvailableOn is null);
        Assert.Contains(courses, course => course.AvailableOn is not null);

        var modules = courses.SelectMany(course => course.Modules).ToArray();
        var lessons = modules.SelectMany(module => module.Lessons).ToArray();
        Assert.Equal(modules.Length, modules.Select(module => module.Id).Distinct().Count());
        Assert.Equal(lessons.Length, lessons.Select(lesson => lesson.Id).Distinct().Count());
        Assert.True(lessons.Select(lesson => lesson.Description).Distinct().Count() > 1);
        Assert.All(courses, course => Assert.All(course.Modules, module =>
        {
            Assert.Equal(course.Id, module.CourseId);
            Assert.False(string.IsNullOrWhiteSpace(module.Description));
            Assert.All(module.Lessons, lesson =>
            {
                Assert.Equal(module.Id, lesson.CourseModuleId);
                Assert.False(string.IsNullOrWhiteSpace(lesson.Title));
                Assert.False(string.IsNullOrWhiteSpace(lesson.Description));
            });
        }));
    }

    [Fact]
    public void CreatesFreshGraphsWithIdenticalValuesOnEveryCall()
    {
        var first = CatalogSeedData.CreateCourses();
        var second = CatalogSeedData.CreateCourses();
        Assert.Equal(JsonSerializer.Serialize(first), JsonSerializer.Serialize(second));
        Assert.Equal(JsonSerializer.Serialize(CatalogSeedData.CreateCategories()), JsonSerializer.Serialize(CatalogSeedData.CreateCategories()));
        Assert.Equal(JsonSerializer.Serialize(CatalogSeedData.CreateModalities()), JsonSerializer.Serialize(CatalogSeedData.CreateModalities()));
        Assert.Equal(JsonSerializer.Serialize(CatalogSeedData.CreateStatuses()), JsonSerializer.Serialize(CatalogSeedData.CreateStatuses()));
        Assert.NotSame(first[0], second[0]);
        Assert.NotSame(first[0].Modules.First(), second[0].Modules.First());
    }
}
