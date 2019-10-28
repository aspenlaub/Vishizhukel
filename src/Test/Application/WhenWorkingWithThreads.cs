using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithThreads {
        [TestMethod]
        public void CanIdentifyMainThread() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            Assert.IsTrue(context.Controller.IsMainThread());
        }

        [TestMethod]
        public void CanIdentifySecondaryThread() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            var controllerCanIdentifyNonMainThread = ControllerCanIdentifyNonMainThread(context);
            Assert.IsTrue(controllerCanIdentifyNonMainThread.Result);
        }

        internal async Task<bool> ControllerCanIdentifyNonMainThread(ApplicationCommandControllerTestExecutionContext context) {
            var notMainThread = false;
            await Task.Run(() => { notMainThread = !context.Controller.IsMainThread(); });
            return notMainThread;
        }
    }
}
