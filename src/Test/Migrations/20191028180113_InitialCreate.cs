using Microsoft.EntityFrameworkCore.Migrations;
// ReSharper disable ArgumentsStyleStringLiteral
// ReSharper disable ArgumentsStyleAnonymousFunction
// ReSharper disable RedundantArgumentDefaultValue

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Migrations {
    // ReSharper disable once UnusedMember.Global
    public partial class InitialCreate : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "TestDatas",
                columns: table => new {
                    Guid = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_TestDatas", x => x.Guid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "TestDatas");
        }
    }
}
