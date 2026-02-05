using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithFakeCommand {
        protected ISimpleLogger SimpleLogger;

        [TestInitialize]
        public void Initialize() {
            var container = new ContainerBuilder().UsePegh("Vishizhukel").Build();
            SimpleLogger = container.Resolve<ISimpleLogger>();
        }

        [TestMethod]
        public async Task DefaultEnabledCommandIsDisabledWhileExecuting() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            var command = new FakeCommand(true, executionContext.Controller);
            Assert.IsFalse(command.WasExecuted);
            executionContext.Controller.AddCommand(command, true);
            Assert.IsTrue(await executionContext.Controller.EnabledAsync(typeof(FakeCommand)));
            await executionContext.Controller.ExecuteAsync(typeof(FakeCommand));
            Assert.IsTrue(command.WasExecuted);
            Assert.IsTrue(await executionContext.Controller.EnabledAsync(typeof(FakeCommand)));
        }

        [TestMethod]
        public async Task DefaultDisabledCommandIsDisabledWhileExecuting() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            var command = new FakeCommand(true, executionContext.Controller);
            Assert.IsFalse(command.WasExecuted);
            executionContext.Controller.AddCommand(command, false);
            Assert.IsTrue(!await executionContext.Controller.EnabledAsync(typeof(FakeCommand)));
            await executionContext.Controller.EnableCommandAsync(typeof(FakeCommand));
            await executionContext.Controller.ExecuteAsync(typeof(FakeCommand));
            Assert.IsTrue(command.WasExecuted);
            Assert.IsTrue(await executionContext.Controller.EnabledAsync(typeof(FakeCommand)));
            await executionContext.Controller.DisableCommandAsync(typeof(FakeCommand));
            Assert.IsTrue(!await executionContext.Controller.EnabledAsync(typeof(FakeCommand)));
        }
    }
}
