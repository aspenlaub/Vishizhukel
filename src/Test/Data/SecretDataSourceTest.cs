using System;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data {
    [TestClass]
    public class SecretDataSourceTest {
        private readonly IContainer _Container;

        public SecretDataSourceTest() {
            _Container = new ContainerBuilder().UsePegh("Vishizhukel").Build();
        }

        [TestMethod]
        public async Task CanGetSecretConnectionStrings() {
            var repository = _Container.Resolve<ISecretRepository>();
            var secretDataSources = new SecretDataSources();
            var errorsAndInfos = new ErrorsAndInfos();
            var dataSources = await repository.GetAsync(secretDataSources, errorsAndInfos);
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));
            Assert.IsTrue(dataSources.Any(d => d.MachineId == Environment.MachineName), $"Missing entry for {Environment.MachineName}");
            Assert.IsTrue(dataSources.All(d => d.TheDataSource.EndsWith("SQLEXPRESS")));
        }
    }
}
