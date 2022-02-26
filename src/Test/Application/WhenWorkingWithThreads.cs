using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithThreads {
        [TestMethod]
        public void CanIdentifyMainThread() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext();
            Assert.IsTrue(executionContext.Controller.IsMainThread());
        }

        [TestMethod]
        public void CanIdentifySecondaryThread() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext();
            var controllerCanIdentifyNonMainThread = ControllerCanIdentifyNonMainThread(executionContext);
            Assert.IsTrue(controllerCanIdentifyNonMainThread.Result);
        }

        internal async Task<bool> ControllerCanIdentifyNonMainThread(ApplicationCommandControllerTestExecutionContext context) {
            var notMainThread = false;
            await Task.Run(() => { notMainThread = !context.Controller.IsMainThread(); });
            return notMainThread;
        }
    }
}
