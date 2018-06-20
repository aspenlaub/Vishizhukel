using System.Data.Entity;
using System.Threading;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core.Data;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Migrations;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data {
    public class TestContext : ContextBase {
        private const string Namespace = "Aspenlaub.Net.GitHub.CSharp.Vishizhukel";

        public DbSet<TestData> TestDatas { get; set; }

        public TestContext() : this(SynchronizationContext.Current) {
        }

        public TestContext(SynchronizationContext uiSynchronizationContext) : base(EnvironmentType.UnitTest, uiSynchronizationContext, Namespace, new ConnectionStringInfos()) {
        }

        public static void SetAutoMigration() {
            SetAutoMigration<TestContext, Configuration>();
        }
    }
}
