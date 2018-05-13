using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class ApplicationDataFolderTest {
        [TestMethod]
        public void ApplicationDataFolderIsSet() {
            var repository = new SecretRepository(new ComponentProvider());
            var applicationDataFolderSecret = new SecretApplicationDataFolder();
            var errorsAndInfos = new ErrorsAndInfos();
            repository.Get(applicationDataFolderSecret, errorsAndInfos);
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));
        }
    }
}
