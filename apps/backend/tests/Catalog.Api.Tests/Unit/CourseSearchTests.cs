using Catalog.Api.Services;
using Xunit;

namespace Catalog.Api.Tests.Unit;

[Trait("Category", "Unit")]
public sealed class CourseSearchTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("   ", null)]
    [InlineData("\t\r\n", null)]
    [InlineData("  Curso  ", "%Curso%")]
    [InlineData("Aula  1", "%Aula  1%")]
    [InlineData("Educação", "%Educação%")]
    [InlineData("a%b", @"%a\%b%")]
    [InlineData("a_b", @"%a\_b%")]
    [InlineData(@"a\b", @"%a\\b%")]
    [InlineData(@" %_\ ", @"%\%\_\\%")]
    public void CreatesLiteralSubstringPatternWithoutChangingOtherCharacters(string? input, string? expected)
    {
        Assert.Equal(expected, CourseSearch.CreatePattern(input));
    }
}
