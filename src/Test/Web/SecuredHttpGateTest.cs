using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Web {
    [TestClass]
    public class SecuredHttpGateTest {
        private readonly IContainer vContainer;

        protected ISecuredHttpGate Sut;
        protected IHttpGate HttpGate;
        protected Uri NonsenseUri;
        protected string ValidMarkup, MarkupWithoutCompatibilityTag;

        public SecuredHttpGateTest() {
            vContainer = new ContainerBuilder().UseVishizhukelDvinAndPegh(new DummyCsArgumentPrompter()).Build();
        }

        [TestInitialize]
        public void Initialize() {
            HttpGate = vContainer.Resolve<IHttpGate>();
            Sut = vContainer.Resolve<ISecuredHttpGate>();
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
