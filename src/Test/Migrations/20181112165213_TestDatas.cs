using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Migrations {
    // ReSharper disable once UnusedMember.Global
    public partial class TestDatas : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                "TestDatas",
                table => new {
                    Guid = table.Column<string>(),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_TestDatas", x => x.Guid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                "TestDatas");
        }
    }
}
