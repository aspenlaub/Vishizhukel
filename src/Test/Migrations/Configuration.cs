using System.Data.Entity.Migrations;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Migrations {
    internal sealed class Configuration : DbMigrationsConfiguration<Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data.TestContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data.TestContext";
        }

        protected override void Seed(Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data.TestContext context) {
        }
    }
}
