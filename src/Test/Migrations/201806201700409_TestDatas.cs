using System.Data.Entity.Migrations;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Migrations {

    public partial class TestDatas : DbMigration {
        public override void Up() {
            CreateTable(
                "dbo.TestDatas",
                c => new {
                    Guid = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Guid);

        }

        public override void Down() {
            DropTable("dbo.TestDatas");
        }
    }
}
