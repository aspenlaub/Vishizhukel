using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Web {
    [TestClass]
    public class SecuredHttpGateTest {
        private readonly IContainer _Container;

        protected ISecuredHttpGate Sut;
        protected IHttpGate HttpGate;
        protected Uri NonsenseUri;
        protected string ValidMarkup, MarkupWithUnclosedElement;

        public SecuredHttpGateTest() {
            _Container = new ContainerBuilder().UseVishizhukelDvinAndPeghAsync("Vishizhukel", new DummyCsArgumentPrompter()).Result.Build();
        }

        [TestInitialize]
        public void Initialize() {
            HttpGate = _Container.Resolve<IHttpGate>();
            Sut = _Container.Resolve<ISecuredHttpGate>();
            NonsenseUri = new Uri(@"http://localhost/this/url/is/nonsense.php");
            ValidMarkup = "<html><head><meta http-equiv=\"X-UA-Compatible\" content=\"IE =edge,chrome=1\" ></head><body></body></html>";
            MarkupWithUnclosedElement = "<html><head></head><body><p></body></html>";
        }

        [TestMethod]
        public async Task ValidHtmlIsValid() {
            if (!await HttpGate.IsLocalHostAvailableAsync()) { return; }

            var result = await Sut.IsHtmlMarkupValidAsync(ValidMarkup);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public async Task HtmlWithUnclosedElementIsInvalid() {
            if (!await HttpGate.IsLocalHostAvailableAsync()) { return; }

            var result = await Sut.IsHtmlMarkupValidAsync(MarkupWithUnclosedElement);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unclosed element OpeningTag[elementId=p]", result.ErrorMessage);
        }

        [TestMethod]
        public async Task CanRegisterDefect() {
            if (!await HttpGate.AreWeOnlineAsync()) { return; }

            var okay = await Sut.RegisterDefectAsync("Vishizhukel's test defect", "Please close this defect if you see it", true);
            Assert.IsTrue(okay);
        }
    }
}
