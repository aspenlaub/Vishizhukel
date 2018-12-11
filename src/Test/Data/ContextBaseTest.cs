using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data {
    [TestClass]
    public class ContextBaseTest {
        [TestMethod]
        public void CanWorkWithContextBase() {
            using (var context = new TestContext(SynchronizationContext.Current)) {
                context.Migrate();
                var dataCount = context.TestDatas.Count();
                if (dataCount >= 10) {
                    context.TestDatas.RemoveRange(context.TestDatas.Take(dataCount - 9));
                }

                var testData = new TestData();
                context.Add(testData);
                context.SaveChanges();
            }
        }
    }
}
