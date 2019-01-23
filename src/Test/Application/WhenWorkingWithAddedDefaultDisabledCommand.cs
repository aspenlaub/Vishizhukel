using System.Threading;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithAddedDefaultDisabledCommand {
        [TestMethod]
        public void ThenCommandIsDisabled() {
            using (var context = new ApplicationCommandControllerTestExecutionContext()) {
                context.Controller.AddCommand(new PrimeNumbersCommand(), false);
                Assert.IsFalse(context.Controller.Enabled(typeof(PrimeNumbersCommand)));
            }
        }

        [TestMethod]
        public async Task WhenCommandCanBeReDisabled() {
            using (var context = new ApplicationCommandControllerTestExecutionContext()) {
                context.Controller.AddCommand(new PrimeNumbersCommand(), false);
                await context.Controller.EnableCommand(typeof(PrimeNumbersCommand));
                await context.Controller.DisableCommand(typeof(PrimeNumbersCommand));
                Assert.IsFalse(context.Controller.Enabled(typeof(PrimeNumbersCommand)));
            }
        }

        [TestMethod]
        public async Task ThenCommandCannotBeReDisabledIfEnabledTwice() {
            using (var context = new ApplicationCommandControllerTestExecutionContext()) {
                context.Controller.AddCommand(new PrimeNumbersCommand(), false);
                await context.Controller.EnableCommand(typeof(PrimeNumbersCommand));
                await context.Controller.EnableCommand(typeof(PrimeNumbersCommand));
                await context.Controller.DisableCommand(typeof(PrimeNumbersCommand));
                Assert.IsTrue(context.Controller.Enabled(typeof(PrimeNumbersCommand)));
            }
        }

        [TestMethod]
        public void AddedDefaultDisabledCommandCanBeEnabledViaFeedback() {
            using (var context = new ApplicationCommandControllerTestExecutionContext()) {
                context.Controller.AddCommand(new PrimeNumbersCommand(), false);
                Assert.IsFalse(context.Controller.Enabled(typeof(PrimeNumbersCommand)));
                var feedback = new FeedbackToApplication() { Type = FeedbackType.EnableCommand, CommandType = typeof(PrimeNumbersCommand) };
                context.Context.Report(feedback);
                Thread.Sleep(ApplicationCommandControllerTestExecutionContext.MillisecondsToWaitForFeedbackToReturn);
                Assert.IsTrue(context.Controller.Enabled(typeof(PrimeNumbersCommand)));
            }
        }

        [TestMethod]
        public async Task ThenCommandCanBeEnabled() {
            using (var context = new ApplicationCommandControllerTestExecutionContext()) {
                context.Controller.AddCommand(new PrimeNumbersCommand(), false);
                await context.Controller.EnableCommand(typeof(PrimeNumbersCommand));
                Assert.IsTrue(context.Controller.Enabled(typeof(PrimeNumbersCommand)));
            }
        }
    }
}