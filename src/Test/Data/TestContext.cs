using System.Threading;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Data;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data {
    public class TestContext : ContextBase {
        private const string Namespace = "Aspenlaub.Net.GitHub.CSharp.Vishizhukel";

        public DbSet<TestData> TestDatas { get; set; }

        // ReSharper disable once UnusedMember.Global
        public TestContext() : this(SynchronizationContext.Current) {
        }

        public TestContext(SynchronizationContext uiSynchronizationContext) : base(EnvironmentType.UnitTest, uiSynchronizationContext, Namespace, new ConnectionStringInfos(), GetDataSources()) {
        }

        private static DataSources GetDataSources() {
            var container = new ContainerBuilder().UsePegh("Vishizhukel", new DummyCsArgumentPrompter()).Build();
            var secretRepository = container.Resolve<ISecretRepository>();
            var secretDataSources = new SecretDataSources();
            var errorsAndInfos = new ErrorsAndInfos();
            var dataSources = secretRepository.GetAsync(secretDataSources, errorsAndInfos).Result;
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));
            return dataSources;
        }
    }
}
