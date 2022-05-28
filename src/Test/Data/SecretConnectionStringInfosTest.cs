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
    public class SecretConnectionStringInfosTest {
        private readonly IContainer Container;

        public SecretConnectionStringInfosTest() {
            Container = new ContainerBuilder().UsePegh("Vishizhukel", new DummyCsArgumentPrompter()).Build();
        }

        [TestMethod]
        public async Task CanGetSecretConnectionStrings() {
            var repository = Container.Resolve<ISecretRepository>();
            var connectionStringInfosSecret = new SecretConnectionStringInfos();
            var errorsAndInfos = new ErrorsAndInfos();
            var connectionStringInfos = await repository.GetAsync(connectionStringInfosSecret, errorsAndInfos);
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));
            Assert.IsTrue(connectionStringInfos.Any(c => c.Namespace == "Aspenlaub.Net.Web" && c.EnvironmentType == EnvironmentType.Production));
        }
    }
}
