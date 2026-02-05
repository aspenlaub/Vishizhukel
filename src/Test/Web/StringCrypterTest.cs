using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Web {
    [TestClass]
    public class StringCrypterTest {
        private readonly IContainer _Container;

        public StringCrypterTest() {
            _Container = new ContainerBuilder().UsePegh("Vishizhukel").Build();
        }

        [TestMethod]
        public async Task CanEncryptAndDecryptStrings() {
            var sampleStrings = new List<string>() { "W", "Wolfgang", "Ärger", "50€", "Schnipp ✂", "❤ Love" };
            var crypter = _Container.Resolve<IStringCrypter>();
            foreach (var s in sampleStrings) {
                var sEncrypted = await crypter.EncryptAsync(s);
                var sDecrypted = await crypter.DecryptAsync(sEncrypted);
                Assert.AreEqual(s, sDecrypted);
            }
        }
    }
}
