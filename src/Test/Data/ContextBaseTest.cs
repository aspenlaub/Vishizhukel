using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data {
    [TestClass]
    public class ContextBaseTest {
        [TestMethod]
        public void CanWorkWithContextBase() {
            TestContext.SetAutoMigration();
            using (var context = new TestContext(SynchronizationContext.Current)) {
                context.Initialise(true);
                var dataCount = context.TestDatas.Count();
                if (dataCount >= 10) {
                    context.TestDatas.RemoveRange(context.TestDatas.Take(dataCount - 9));
                }

                var testData = new TestData();
                context.TestDatas.AddOrUpdate(t => t.Guid, testData);
                context.SaveChanges();
            }
        }
    }
}
