using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Web {
    [TestClass]
    public class SecuredHttpGateTest {
        protected ISecuredHttpGate Sut;
        protected IHttpGate HttpGate;
        protected Uri NonsenseUri;
        protected string ValidMarkup, MarkupWithoutCompatibilityTag;

        [TestInitialize]
        public void Initialize() {
            HttpGate = new HttpGate();

            var repository = new SecretRepository(new ComponentProvider());
            var securedHttpGateSettingsSecret = new SecretSecuredHttpGateSettings();
            var errorsAndInfos = new ErrorsAndInfos();
            var securedHttpGateSettings = repository.GetAsync(securedHttpGateSettingsSecret, errorsAndInfos).Result;
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));

            Sut = new SecuredHttpGate(HttpGate, securedHttpGateSettings);
            NonsenseUri = new Uri(@"http://localhost/this/url/is/nonsense.php");
            ValidMarkup = "<html><head><meta http-equiv=\"X-UA-Compatible\" content=\"IE =edge,chrome=1\" ></head><body></body></html>";
            MarkupWithoutCompatibilityTag = "<html><head></head><body></body></html>";
        }

        [TestMethod]
        public async Task ValidHtmlIsValid() {
            if (!await HttpGate.IsLocalHostAvailableAsync()) { return; }

            var result = await Sut.IsHtmlMarkupValidAsync(ValidMarkup);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public async Task HtmlWithoutCompatibilityTagIsInvalid() {
            if (!await HttpGate.IsLocalHostAvailableAsync()) { return; }

            var result = await Sut.IsHtmlMarkupValidAsync(MarkupWithoutCompatibilityTag);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Missing compatibility tag", result.ErrorMessage);
        }

        [TestMethod]
        public async Task CanRegisterDefect() {
            if (!await HttpGate.AreWeOnlineAsync()) { return; }

            var okay = await Sut.RegisterDefectAsync("Vishizhukel's test defect", "Please close this defect if you see it", true);
            Assert.IsTrue(okay);
        }
    }
}
