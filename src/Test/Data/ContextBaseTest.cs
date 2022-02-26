using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data {
    [TestClass]
    public class ContextBaseTest {
        [TestMethod]
        public void CanWorkWithContextBase() {
            using var testContext = new TestContext(SynchronizationContext.Current);
            testContext.Migrate();
            var dataCount = testContext.TestDatas.Count();
            if (dataCount >= 10) {
                testContext.TestDatas.RemoveRange(testContext.TestDatas.Take(dataCount - 9));
            }

            var testData = new TestData();
            testContext.Add(testData);
            testContext.SaveChanges();
        }
    }
}
