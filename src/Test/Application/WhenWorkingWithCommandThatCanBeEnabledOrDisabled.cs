using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithCommandThatCanBeEnabledOrDisabled {
        [TestMethod]
        public void CommandIsNotEnabledForNewController() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            Assert.IsFalse(context.Controller.Enabled(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public void OnCommandEnabledOrDisabledNotCalledInitially() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            Assert.IsFalse(context.CommandsEnabledOrDisabledWasReported);
        }

        [TestMethod]
        public void OnCommandEnabledOrDisabledNotCalledWhenAddingACommand() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), true);
            Assert.IsFalse(context.CommandsEnabledOrDisabledWasReported);
        }

        [TestMethod]
        public async Task OnCommandEnabledOrDisabledIsCalledWhenDisablingACommand() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), true);
            await context.Controller.DisableCommand(typeof(PrimeNumbersCommand));
            Assert.IsTrue(context.CommandsEnabledOrDisabledWasReported);
        }

        [TestMethod]
        public async Task OnCommandEnabledOrDisabledIsCalledWhenReEnablingACommand() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), true);
            await context.Controller.DisableCommand(typeof(PrimeNumbersCommand));
            context.CommandsEnabledOrDisabledWasReported = false;
            await context.Controller.EnableCommand(typeof(PrimeNumbersCommand));
            Assert.IsTrue(context.CommandsEnabledOrDisabledWasReported);
        }

        [TestMethod]
        public async Task OnCommandEnabledOrDisabledIsCalledWhenDisablingACommandTwice() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), true);
            await context.Controller.DisableCommand(typeof(PrimeNumbersCommand));
            context.CommandsEnabledOrDisabledWasReported = false;
            await context.Controller.DisableCommand(typeof(PrimeNumbersCommand));
            Assert.IsFalse(context.CommandsEnabledOrDisabledWasReported);
        }
    }
}
