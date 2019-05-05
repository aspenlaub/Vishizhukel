using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Web {
    [TestClass]
    public class SecretSecuredHttpGateSettingsTest {
        [TestMethod]
        public async Task CanGetSecretSecuredHttpGateSettings() {
            var repository = new SecretRepository(new ComponentProvider());
            var securedHttpGateSettingsSecret = new SecretSecuredHttpGateSettings();
            var errorsAndInfos = new ErrorsAndInfos();
            var securedHttpGateSettings = await repository.GetAsync(securedHttpGateSettingsSecret, errorsAndInfos);
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));

            var componentProvider = new ComponentProvider();
            var folder = componentProvider.FolderResolver.Resolve(securedHttpGateSettings.LocalhostTempPath, errorsAndInfos);
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));
            var file = Directory.GetFiles(folder.FullName, "*.txt").FirstOrDefault();
            if (string.IsNullOrEmpty(file)) { return; }

            var contents = File.ReadAllText(file);
            var shortFileName = file.Substring(file.LastIndexOf('\\') + 1);
            var httpGate = new HttpGate();
            var httpResponseMessage = await httpGate.GetAsync(new Uri(securedHttpGateSettings.LocalhostTempPathUrl + shortFileName));
            var httpContents = await httpResponseMessage.Content.ReadAsStringAsync();
            Assert.AreEqual(contents, httpContents);
        }
    }
}
