using System.Text.Json;
using Catalog.Api.Content;
using Xunit;

namespace Catalog.Api.Tests.Unit;

[Trait("Category", "Unit")]
public sealed class CoursePresentationTests
{
    [Fact]
    public void RoundTripsDetailsAndLearningPreviewContent()
    {
        var json = CoursePresentationSerializer.Serialize(CreatePresentation());
        var presentation = CoursePresentationSerializer.Deserialize(json);

        Assert.Equal("SESI", presentation.Certification);
        Assert.Equal("/assets/video-thumb.png", presentation.DetailsImagePath);
        Assert.Equal(10, presentation.Faqs.Count);
        Assert.Equal(10, presentation.LearningPreview.ProgressPercent);
        Assert.Equal("1.2 Boas práticas", presentation.LearningPreview.Introduction.Items[1].Label);
        Assert.Equal("current", presentation.LearningPreview.Introduction.Items[1].State);
        Assert.Equal(6, presentation.LearningPreview.Modules.Count);
        Assert.Equal(2, presentation.LearningPreview.Modules[0].Lessons.Count);
        Assert.Equal("2.1 Introdução ao módulo", presentation.LearningPreview.Modules[0].Lessons[0].Label);
        Assert.Equal("Descrição demonstrativa.", presentation.LearningPreview.Modules[0].Lessons[0].Description);
        Assert.Equal(3, presentation.LearningPreview.Article.Paragraphs.Count);
        Assert.Equal(4, presentation.LearningPreview.Recommendations.Paragraphs.Count);
        Assert.Equal(json, CoursePresentationSerializer.Serialize(presentation));
    }

    [Fact]
    public void RejectsMissingRequiredPresentationContent()
    {
        const string json = """
            {
              "detailsImagePath": "/assets/video-thumb.png",
              "onlineLessons": {},
              "assessments": {},
              "faqs": [],
              "learningPreview": {}
            }
            """;

        Assert.Throws<JsonException>(() => CoursePresentationSerializer.Deserialize(json));
    }

    [Fact]
    public void RejectsAVAModuleWithoutItsTwoGenericLessons()
    {
        var presentation = CreatePresentation();
        presentation.LearningPreview.Modules[0] = new LearningModule
        {
            Label = "2. Módulo 1",
            Expanded = false,
            Lessons = []
        };

        Assert.Throws<JsonException>(() => CoursePresentationSerializer.Serialize(presentation));
    }

    [Fact]
    public void RejectsHtmlAndUnknownStudentData()
    {
        Assert.Throws<JsonException>(() => CoursePresentationSerializer.Deserialize(
            CoursePresentationSerializer.Serialize(CreatePresentation("<p>Aulas em módulos.</p>"))));

        Assert.Throws<JsonException>(() => CoursePresentationSerializer.Deserialize(
            CoursePresentationSerializer.Serialize(CreatePresentation())
                .Insert(1, "\"student\":{\"id\":1},")));
    }

    private static CoursePresentation CreatePresentation(string onlineIntroduction = "Aulas em módulos.") => new()
    {
        Certification = "SESI",
        DetailsImagePath = "/assets/video-thumb.png",
        OnlineLessons = new CourseTextSection
        {
            Introduction = onlineIntroduction,
            Topics = ["Vídeos técnicos."],
            Accessibility = "Legendas disponíveis."
        },
        Assessments = new CourseTextSectionWithConclusion
        {
            Introduction = "Avaliações demonstrativas.",
            Topics = ["Exercícios de aplicação."],
            Conclusion = "Conteúdo de exemplo."
        },
        Faqs = Enumerable.Range(1, 10).Select(index => new CourseFaq
        {
            Question = $"Pergunta {index}?",
            Answer = $"Resposta {index}."
        }).ToList(),
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
            Modules = Enumerable.Range(1, 6).Select(index => new LearningModule
            {
                Label = $"{index + 1}. Módulo {index}",
                Expanded = false,
                Lessons =
                [
                    new LearningLesson { Label = $"{index + 1}.1 Introdução ao módulo", Description = "Descrição demonstrativa." },
                    new LearningLesson { Label = $"{index + 1}.2 Revisão dos conceitos", Description = "Descrição demonstrativa." }
                ]
            }).ToList(),
            Article = new LearningArticle
            {
                Title = "Boas práticas",
                Paragraphs = ["Parágrafo 1.", "Parágrafo 2.", "Parágrafo 3."]
            },
            Recommendations = new LearningArticle
            {
                Title = "Recomendações",
                Paragraphs = ["Parágrafo 1.", "Parágrafo 2.", "Parágrafo 3.", "Parágrafo 4."]
            }
        }
    };
}
