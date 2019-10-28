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
            Assert.IsTrue(context.Controller.Enabled(typeof(FakeCommand)));
            await context.Controller.Execute(typeof(FakeCommand));
            await context.Controller.AwaitAllAsynchronousTasks();
            Assert.IsTrue(command.WasExecuted);
            Assert.IsTrue(context.Controller.Enabled(typeof(FakeCommand)));
        }

        [TestMethod]
        public async Task DefaultDisabledCommandIsDisabledWhileExecuting() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            var command = new FakeCommand(true, context.Controller);
            Assert.IsFalse(command.WasExecuted);
            context.Controller.AddCommand(command, false);
            Assert.IsTrue(!context.Controller.Enabled(typeof(FakeCommand)));
            await context.Controller.EnableCommand(typeof(FakeCommand));
            await context.Controller.Execute(typeof(FakeCommand));
            await context.Controller.AwaitAllAsynchronousTasks();
            Assert.IsTrue(command.WasExecuted);
            Assert.IsTrue(context.Controller.Enabled(typeof(FakeCommand)));
            await context.Controller.DisableCommand(typeof(FakeCommand));
            Assert.IsTrue(!context.Controller.Enabled(typeof(FakeCommand)));
        }
    }
}
