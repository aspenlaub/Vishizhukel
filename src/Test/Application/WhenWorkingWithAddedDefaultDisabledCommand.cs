using System.Threading;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithAddedDefaultDisabledCommand {
        [TestMethod]
        public async Task ThenCommandIsDisabled() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), false);
            Assert.IsFalse(await context.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task WhenCommandCanBeReDisabled() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), false);
            await context.Controller.EnableCommandAsync(typeof(PrimeNumbersCommand));
            await context.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsFalse(await context.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task ThenCommandCannotBeReDisabledIfEnabledTwice() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), false);
            await context.Controller.EnableCommandAsync(typeof(PrimeNumbersCommand));
            await context.Controller.EnableCommandAsync(typeof(PrimeNumbersCommand));
            await context.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsTrue(await context.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task AddedDefaultDisabledCommandCanBeEnabledViaFeedback() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), false);
            Assert.IsFalse(await context.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
            var feedback = new FeedbackToApplication() { Type = FeedbackType.EnableCommand, CommandType = typeof(PrimeNumbersCommand) };
            context.Context.Report(feedback);
            Thread.Sleep(ApplicationCommandControllerTestExecutionContext.MillisecondsToWaitForFeedbackToReturn);
            Assert.IsTrue(await context.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task ThenCommandCanBeEnabled() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            context.Controller.AddCommand(new PrimeNumbersCommand(), false);
            await context.Controller.EnableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsTrue(await context.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }
    }
}