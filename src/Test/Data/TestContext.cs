using System.Threading;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Data;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data;
using Microsoft.EntityFrameworkCore;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data {
    public class TestContext : ContextBase {
        private const string Namespace = "Aspenlaub.Net.GitHub.CSharp.Vishizhukel";

        public DbSet<TestData> TestDatas { get; set; }

        // ReSharper disable once UnusedMember.Global
        public TestContext() : this(SynchronizationContext.Current) {
        }

        public TestContext(SynchronizationContext uiSynchronizationContext) : base(EnvironmentType.UnitTest, uiSynchronizationContext, Namespace, new ConnectionStringInfos()) {
        }
    }
}
