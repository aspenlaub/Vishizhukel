using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Web {
    [TestClass]
    public class SecretSecuredHttpGateSettingsTest {
        private readonly IContainer vContainer;

        public SecretSecuredHttpGateSettingsTest() {
            vContainer = new ContainerBuilder().UseVishizhukelAndPegh(new DummyCsArgumentPrompter()).Build();
        }

        [TestMethod]
        public async Task CanGetSecretSecuredHttpGateSettings() {
            var repository = vContainer.Resolve<ISecretRepository>();
            var securedHttpGateSettingsSecret = new SecretSecuredHttpGateSettings();
            var errorsAndInfos = new ErrorsAndInfos();
            var securedHttpGateSettings = await repository.GetAsync(securedHttpGateSettingsSecret, errorsAndInfos);
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));

            var folder = vContainer.Resolve<IFolderResolver>().Resolve(securedHttpGateSettings.LocalhostTempPath, errorsAndInfos);
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));
            var file = Directory.GetFiles(folder.FullName, "*.txt").FirstOrDefault();
            if (string.IsNullOrEmpty(file)) { return; }

            var contents = File.ReadAllText(file);
            var shortFileName = file.Substring(file.LastIndexOf('\\') + 1);
            var httpGate = vContainer.Resolve<IHttpGate>();
            var httpResponseMessage = await httpGate.GetAsync(new Uri(securedHttpGateSettings.LocalhostTempPathUrl + shortFileName));
            var httpContents = await httpResponseMessage.Content.ReadAsStringAsync();
            Assert.AreEqual(contents, httpContents);
        }
    }
}
