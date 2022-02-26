using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithAddedDefaultEnabledCommand {
        [TestMethod]
        public async Task ThenCommandIsEnabled() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), true);
            Assert.IsTrue(await context.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task ThenCommandCanBeDisabled() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), true);
            await context.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsFalse(await context.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task ThenCommandCanBeReEnabled() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), true);
            await context.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            await context.Controller.EnableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsTrue(await context.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task ThenCommandCannotBeReEnabledIfDisabledTwice() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), true);
            await context.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            await context.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            await context.Controller.EnableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsFalse(await context.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task ThenCommandCanBeDisabledViaFeedback() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), true);
            Assert.IsTrue(await context.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
            var feedback = new FeedbackToApplication() { Type = FeedbackType.DisableCommand, CommandType = typeof(PrimeNumbersCommand) };
            context.Context.Report(feedback);
            Thread.Sleep(ApplicationCommandControllerTestExecutionContext.MillisecondsToWaitForFeedbackToReturn);
            Assert.IsFalse(await context.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }
    }
}