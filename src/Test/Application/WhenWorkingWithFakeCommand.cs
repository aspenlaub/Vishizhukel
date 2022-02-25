using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithFakeCommand {
        [TestMethod]
        public async Task DefaultEnabledCommandIsDisabledWhileExecuting() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            var command = new FakeCommand(true, context.Controller);
            Assert.IsFalse(command.WasExecuted);
            context.Controller.AddCommand(command, true);
            Assert.IsTrue(await context.Controller.EnabledAsync(typeof(FakeCommand)));
            await context.Controller.ExecuteAsync(typeof(FakeCommand));
            await context.Controller.AwaitAllAsynchronousTasks();
            Assert.IsTrue(command.WasExecuted);
            Assert.IsTrue(await context.Controller.EnabledAsync(typeof(FakeCommand)));
        }

        [TestMethod]
        public async Task DefaultDisabledCommandIsDisabledWhileExecuting() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            var command = new FakeCommand(true, context.Controller);
            Assert.IsFalse(command.WasExecuted);
            context.Controller.AddCommand(command, false);
            Assert.IsTrue(!await context.Controller.EnabledAsync(typeof(FakeCommand)));
            await context.Controller.EnableCommand(typeof(FakeCommand));
            await context.Controller.ExecuteAsync(typeof(FakeCommand));
            await context.Controller.AwaitAllAsynchronousTasks();
            Assert.IsTrue(command.WasExecuted);
            Assert.IsTrue(await context.Controller.EnabledAsync(typeof(FakeCommand)));
            await context.Controller.DisableCommand(typeof(FakeCommand));
            Assert.IsTrue(!await context.Controller.EnabledAsync(typeof(FakeCommand)));
        }
    }
}
