using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithThreads {
        protected ISimpleLogger SimpleLogger;

        [TestInitialize]
        public void Initialize() {
            var container = new ContainerBuilder().UsePegh("Vishizhukel").Build();
            SimpleLogger = container.Resolve<ISimpleLogger>();
        }

        [TestMethod]
        public void CanIdentifyMainThread() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            Assert.IsTrue(executionContext.Controller.IsMainThread());
        }

        [TestMethod]
        public void CanIdentifySecondaryThread() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
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
