using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Web {
    [TestClass]
    public class StringCrypterTest {
        [TestMethod]
        public void CanEncryptAndDecryptStrings() {
            var sampleStrings = new List<string>() { "W", "Wolfgang", "Ärger", "50€", "Schnipp ✂", "❤ Love" };
            var componentProvider = new ComponentProvider();
            var crypter = componentProvider.StringCrypter;
            foreach (var s in sampleStrings) {
                var sEncrypted = crypter.Encrypt(s);
                var sDecrypted = crypter.Decrypt(sEncrypted);
                Assert.AreEqual(s, sDecrypted);
            }
        }
    }
}
