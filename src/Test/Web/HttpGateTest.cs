using System;
using System.Net;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Web {
    [TestClass]
    public class HttpGateTest {
        protected IHttpGate Sut;
        protected Uri NonsenseUri;

        private readonly IContainer vContainer;

        public HttpGateTest() {
            vContainer = new ContainerBuilder().UseVishizhukelDvinAndPegh(new DummyCsArgumentPrompter()).Build();
        }

        [TestInitialize]
        public void Initialize() {
            Sut = vContainer.Resolve<IHttpGate>();
            NonsenseUri = new Uri(@"http://localhost/this/url/is/nonsense.php");
        }

        [TestMethod]
        public async Task CannotRequestInvalidUrl() {
            var response = await Sut.GetAsync(NonsenseUri);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.ServiceUnavailable);
        }

        [TestMethod]
        public async Task NonsenseServiceUnavailableIfLocalhostUnavailable() {
            if (await Sut.IsLocalHostAvailableAsync()) { return; }

            var response = await Sut.GetAsync(NonsenseUri);
            Assert.AreEqual(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }

        [TestMethod]
        public async Task NonsenseNotFoundIfLocalhostAvailable() {
            if (!await Sut.IsLocalHostAvailableAsync()) { return; }

            var response = await Sut.GetAsync(NonsenseUri);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
