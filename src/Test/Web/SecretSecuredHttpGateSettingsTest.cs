using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Skladasu.Entities;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Web {
    [TestClass]
    public class SecretSecuredHttpGateSettingsTest {
        private readonly IContainer _Container;

        public SecretSecuredHttpGateSettingsTest() {
            _Container = new ContainerBuilder().UseVishizhukelAndPeghAsync("Vishizhukel").Result.Build();
        }

        [TestMethod]
        public async Task CanGetSecretSecuredHttpGateSettings() {
            ISecretRepository repository = _Container.Resolve<ISecretRepository>();
            var securedHttpGateSettingsSecret = new SecretSecuredHttpGateSettings();
            var errorsAndInfos = new ErrorsAndInfos();
            SecuredHttpGateSettings securedHttpGateSettings = await repository.GetAsync(securedHttpGateSettingsSecret, errorsAndInfos);
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));

            IFolder folder = await _Container.Resolve<IFolderResolver>().ResolveAsync(securedHttpGateSettings.LocalhostTempPath, errorsAndInfos);
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));
            string file = Directory.GetFiles(folder.FullName, "*.txt").FirstOrDefault();
            if (string.IsNullOrEmpty(file)) { return; }

            string contents = await File.ReadAllTextAsync(file);
            string shortFileName = file.Substring(file.LastIndexOf('\\') + 1);
            IHttpGate httpGate = _Container.Resolve<IHttpGate>();
            HttpResponseMessage httpResponseMessage = await httpGate.GetAsync(new Uri(securedHttpGateSettings.LocalhostTempPathUrl + shortFileName));
            string httpContents = await httpResponseMessage.Content.ReadAsStringAsync();
            Assert.AreEqual(contents, httpContents);
        }
    }
}
