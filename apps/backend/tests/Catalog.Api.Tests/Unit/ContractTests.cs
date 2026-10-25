using System.Text.Json;
using Catalog.Api.DTOs;
using Xunit;

namespace Catalog.Api.Tests.Unit;

[Trait("Category", "Unit")]
public sealed class ContractTests
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    [Fact]
    public void AllQueryParametersAreOptional()
    {
        var query = new CourseQueryParameters();
        Assert.Null(query.Search);
        Assert.Null(query.CategoryId);
        Assert.Null(query.ModalityId);
        Assert.Null(query.StatusId);
    }

    [Fact]
    public void SerializesListEnvelopeAndClassificationNamesInCamelCase()
    {
        var summary = new CourseSummaryDto(1, "Curso fictício", "Curso", "Descrição completa", "Descrição breve",
            new(1, "Categoria"),
            new(2, "Modalidade"), new(3, "Status"), 12, "Educadores",
            "https://frontend.example.test/assets/course.png", null);
        var json = JsonSerializer.SerializeToElement(new CourseListResponse([summary], 1), Options);

        Assert.Equal(1, json.GetProperty("totalCount").GetInt32());
        var item = json.GetProperty("items")[0];
        Assert.Equal("Curso fictício", item.GetProperty("fullName").GetString());
        Assert.Equal("Descrição completa", item.GetProperty("description").GetString());
        Assert.Equal("Descrição breve", item.GetProperty("shortDescription").GetString());
        Assert.Equal("Categoria", item.GetProperty("category").GetProperty("name").GetString());
        Assert.Equal(2, item.GetProperty("category").EnumerateObject().Count());
        Assert.Equal(JsonValueKind.Null, item.GetProperty("availableOn").ValueKind);
        Assert.False(item.TryGetProperty("CategoryId", out _));
        Assert.False(item.TryGetProperty("categoryId", out _));
        Assert.False(item.TryGetProperty("modules", out _));
    }

    [Fact]
    public void SerializesCompleteDetailsWithoutPersistenceNavigations()
    {
        var detail = new CourseDetailDto(1, "Curso fictício", "Curso", "Descrição", "Resumo",
            new(1, "Categoria"), new(1, "Modalidade"), new(1, "Status"), 12, "Educadores",
            "https://frontend.example.test/assets/course.png", new DateOnly(2027, 2, 10),
            [new(101, "Módulo", "Descrição do módulo", 1, [new(10101, "Aula", "Descrição da aula", 1)])],
            CreatePresentationDto());
        var json = JsonSerializer.SerializeToElement(detail, Options);

        Assert.Equal("2027-02-10", json.GetProperty("availableOn").GetString());
        Assert.Equal("Resumo", json.GetProperty("shortDescription").GetString());
        var presentation = json.GetProperty("presentation");
        Assert.Equal("SESI", presentation.GetProperty("certification").GetString());
        Assert.Equal("https://frontend.example.test/assets/video-thumb.png",
            presentation.GetProperty("detailsImageUrl").GetString());
        Assert.Equal("https://frontend.example.test/assets/banner-ava.png",
            presentation.GetProperty("learningPreview").GetProperty("bannerImageUrl").GetString());
        var previewModule = presentation.GetProperty("learningPreview").GetProperty("modules")[0];
        Assert.Equal("2.1 Introdução ao módulo", previewModule.GetProperty("lessons")[0].GetProperty("label").GetString());
        Assert.Equal("Descrição da aula AVA.", previewModule.GetProperty("lessons")[0].GetProperty("description").GetString());
        Assert.False(presentation.TryGetProperty("detailsImagePath", out _));
        var module = json.GetProperty("modules")[0];
        Assert.Equal(1, module.GetProperty("order").GetInt32());
        Assert.Equal("Descrição da aula", module.GetProperty("lessons")[0].GetProperty("description").GetString());
        Assert.False(module.TryGetProperty("course", out _));
        Assert.False(module.TryGetProperty("courseId", out _));
        Assert.False(module.GetProperty("lessons")[0].TryGetProperty("module", out _));
    }

    private static CoursePresentationDto CreatePresentationDto() => new(
        "SESI",
        "https://frontend.example.test/assets/video-thumb.png",
        new("Aulas introdutórias.", ["Tópico."], "Acessibilidade."),
        new("Avaliações demonstrativas.", ["Exercício."], "Conclusão informativa."),
        [new("Pergunta?", "Resposta.")],
        new("CERTIFICAÇÃO", 10, "Buscar conteúdo do curso", "https://frontend.example.test/assets/banner-ava.png",
            new("1. Introdução", true, [new("1.1 Introdução", "completed")]),
            [new("2. Módulo 1", false, [new("2.1 Introdução ao módulo", "Descrição da aula AVA.")])],
            new("Boas práticas", ["Parágrafo."]),
            new("Recomendações", ["Recomendação."])));
}
