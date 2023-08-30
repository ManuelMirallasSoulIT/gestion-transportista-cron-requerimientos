using Microsoft.EntityFrameworkCore.Migrations;

namespace gestion_transportista_cron_requerimientos.Infrastructure.Migrations;

public partial class sqldemo : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "person",
            columns: new[] { "PersonId", "Apellido", "Nombre" },
            values: new object[] { 4, "apellidoTest4", "nombreTest4" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "person",
            keyColumn: "PersonId",
            keyValue: 4);
    }
}
