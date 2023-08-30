using Microsoft.EntityFrameworkCore.Migrations;

namespace gestion_transportista_cron_requerimientos.Infrastructure.Migrations;

public partial class sqlinit : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "person",
            columns: table => new
            {
                PersonId = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Nombre = table.Column<string>(nullable: true),
                Apellido = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_person", x => x.PersonId);
            });

        migrationBuilder.InsertData(
            table: "person",
            columns: new[] { "PersonId", "Apellido", "Nombre" },
            values: new object[] { 1, "apellidoTest", "nombreTest" });

        migrationBuilder.InsertData(
            table: "person",
            columns: new[] { "PersonId", "Apellido", "Nombre" },
            values: new object[] { 2, "apellidoTest2", "nombreTest2" });

        migrationBuilder.InsertData(
            table: "person",
            columns: new[] { "PersonId", "Apellido", "Nombre" },
            values: new object[] { 3, "apellidoTest3", "nombreTest3" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "person");
    }
}
