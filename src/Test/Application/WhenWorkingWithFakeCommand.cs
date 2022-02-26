using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithFakeCommand {
        [TestMethod]
        public async Task DefaultEnabledCommandIsDisabledWhileExecuting() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext();
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
            var executionContext = new ApplicationCommandControllerTestExecutionContext();
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
