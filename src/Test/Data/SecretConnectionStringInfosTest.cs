using System.Linq;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data {
    [TestClass]
    public class SecretConnectionStringInfosTest {
        [TestMethod]
        public void CanGetSecretConnectionStrings() {
            var repository = new SecretRepository(new ComponentProvider());
            var connectionStringInfosSecret = new SecretConnectionStringInfos();
            var errorsAndInfos = new ErrorsAndInfos();
            var connectionStringInfos = repository.Get(connectionStringInfosSecret, errorsAndInfos);
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));
            Assert.IsTrue(connectionStringInfos.Any(c => c.Namespace == "Aspenlaub.Net.Web" && c.EnvironmentType == EnvironmentType.Production));
        }
    }
}
