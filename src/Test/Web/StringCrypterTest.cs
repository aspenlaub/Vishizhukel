using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Web {
    [TestClass]
    public class StringCrypterTest {
        private readonly IContainer vContainer;

        public StringCrypterTest() {
            vContainer = new ContainerBuilder().UsePegh(new DummyCsArgumentPrompter()).Build();
        }

        [TestMethod]
        public void CanEncryptAndDecryptStrings() {
            var sampleStrings = new List<string>() { "W", "Wolfgang", "Ärger", "50€", "Schnipp ✂", "❤ Love" };
            var crypter = vContainer.Resolve<IStringCrypter>();
            foreach (var s in sampleStrings) {
                var sEncrypted = crypter.Encrypt(s);
                var sDecrypted = crypter.Decrypt(sEncrypted);
                Assert.AreEqual(s, sDecrypted);
            }
        }
    }
}
