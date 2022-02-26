using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithCommandThatCanBeEnabledOrDisabled {
        [TestMethod]
        public async Task CommandIsNotEnabledForNewController() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext();
            Assert.IsFalse(await executionContext.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public void OnCommandEnabledOrDisabledNotCalledInitially() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext();
            Assert.IsFalse(executionContext.CommandsEnabledOrDisabledWasReported);
        }

        [TestMethod]
        public void OnCommandEnabledOrDisabledNotCalledWhenAddingACommand() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext();
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), true);
            Assert.IsFalse(executionContext.CommandsEnabledOrDisabledWasReported);
        }

        [TestMethod]
        public async Task OnCommandEnabledOrDisabledIsCalledWhenDisablingACommand() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext();
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), true);
            await executionContext.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsTrue(executionContext.CommandsEnabledOrDisabledWasReported);
        }

        [TestMethod]
        public async Task OnCommandEnabledOrDisabledIsCalledWhenReEnablingACommand() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext();
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), true);
            await executionContext.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            executionContext.CommandsEnabledOrDisabledWasReported = false;
            await executionContext.Controller.EnableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsTrue(executionContext.CommandsEnabledOrDisabledWasReported);
        }

        [TestMethod]
        public async Task OnCommandEnabledOrDisabledIsCalledWhenDisablingACommandTwice() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext();
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), true);
            await executionContext.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            executionContext.CommandsEnabledOrDisabledWasReported = false;
            await executionContext.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsFalse(executionContext.CommandsEnabledOrDisabledWasReported);
        }
    }
}
