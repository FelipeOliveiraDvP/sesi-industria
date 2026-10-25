using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGenericAvaLessons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE "Courses"
                SET "PresentationJson" = jsonb_set(
                    "PresentationJson",
                    '{learningPreview,modules}',
                    '[
                      {"label":"2. Módulo 1","expanded":false,"lessons":[{"label":"2.1 Introdução ao módulo","description":"Apresentação demonstrativa dos objetivos e conceitos principais do módulo."},{"label":"2.2 Revisão dos conceitos","description":"Revisão demonstrativa do conteúdo antes de avançar para o próximo módulo."}]},
                      {"label":"3. Módulo 2","expanded":false,"lessons":[{"label":"3.1 Introdução ao módulo","description":"Apresentação demonstrativa dos objetivos e conceitos principais do módulo."},{"label":"3.2 Revisão dos conceitos","description":"Revisão demonstrativa do conteúdo antes de avançar para o próximo módulo."}]},
                      {"label":"4. Módulo 3","expanded":false,"lessons":[{"label":"4.1 Introdução ao módulo","description":"Apresentação demonstrativa dos objetivos e conceitos principais do módulo."},{"label":"4.2 Revisão dos conceitos","description":"Revisão demonstrativa do conteúdo antes de avançar para o próximo módulo."}]},
                      {"label":"5. Módulo 4","expanded":false,"lessons":[{"label":"5.1 Introdução ao módulo","description":"Apresentação demonstrativa dos objetivos e conceitos principais do módulo."},{"label":"5.2 Revisão dos conceitos","description":"Revisão demonstrativa do conteúdo antes de avançar para o próximo módulo."}]},
                      {"label":"6. Módulo 5","expanded":false,"lessons":[{"label":"6.1 Introdução ao módulo","description":"Apresentação demonstrativa dos objetivos e conceitos principais do módulo."},{"label":"6.2 Revisão dos conceitos","description":"Revisão demonstrativa do conteúdo antes de avançar para o próximo módulo."}]},
                      {"label":"7. Módulo 6","expanded":false,"lessons":[{"label":"7.1 Introdução ao módulo","description":"Apresentação demonstrativa dos objetivos e conceitos principais do módulo."},{"label":"7.2 Revisão dos conceitos","description":"Revisão demonstrativa do conteúdo antes de avançar para o próximo módulo."}]}
                    ]'::jsonb,
                    true)
                WHERE "Id" BETWEEN 1 AND 6;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE "Courses"
                SET "PresentationJson" = jsonb_set(
                    "PresentationJson",
                    '{learningPreview,modules}',
                    '[
                      {"label":"2. Módulo 1","expanded":false},
                      {"label":"3. Módulo 2","expanded":false},
                      {"label":"4. Módulo 3","expanded":false},
                      {"label":"5. Módulo 4","expanded":false},
                      {"label":"6. Módulo 5","expanded":false},
                      {"label":"7. Módulo 6","expanded":false}
                    ]'::jsonb,
                    true)
                WHERE "Id" BETWEEN 1 AND 6;
                """);
        }
    }
}
