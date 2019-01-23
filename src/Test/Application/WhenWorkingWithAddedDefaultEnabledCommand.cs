using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithAddedDefaultEnabledCommand {
        [TestMethod]
        public void ThenCommandIsEnabled() {
            using (var context = new ApplicationCommandControllerTestExecutionContext()) {
                context.Controller.AddCommand(new PrimeNumbersCommand(), true);
                Assert.IsTrue(context.Controller.Enabled(typeof(PrimeNumbersCommand)));
            }
        }

        [TestMethod]
        public async Task ThenCommandCanBeDisabled() {
            using (var context = new ApplicationCommandControllerTestExecutionContext()) {
                context.Controller.AddCommand(new PrimeNumbersCommand(), true);
                await context.Controller.DisableCommand(typeof(PrimeNumbersCommand));
                Assert.IsFalse(context.Controller.Enabled(typeof(PrimeNumbersCommand)));
            }
        }

        [TestMethod]
        public async Task ThenCommandCanBeReEnabled() {
            using (var context = new ApplicationCommandControllerTestExecutionContext()) {
                context.Controller.AddCommand(new PrimeNumbersCommand(), true);
                await context.Controller.DisableCommand(typeof(PrimeNumbersCommand));
                await context.Controller.EnableCommand(typeof(PrimeNumbersCommand));
                Assert.IsTrue(context.Controller.Enabled(typeof(PrimeNumbersCommand)));
            }
        }

        [TestMethod]
        public async Task ThenCommandCannotBeReEnabledIfDisabledTwice() {
            using (var context = new ApplicationCommandControllerTestExecutionContext()) {
                context.Controller.AddCommand(new PrimeNumbersCommand(), true);
                await context.Controller.DisableCommand(typeof(PrimeNumbersCommand));
                await context.Controller.DisableCommand(typeof(PrimeNumbersCommand));
                await context.Controller.EnableCommand(typeof(PrimeNumbersCommand));
                Assert.IsFalse(context.Controller.Enabled(typeof(PrimeNumbersCommand)));
            }
        }

        [TestMethod]
        public void ThenCommandCanBeDisabledViaFeedback() {
            using (var context = new ApplicationCommandControllerTestExecutionContext()) {
                context.Controller.AddCommand(new PrimeNumbersCommand(), true);
                Assert.IsTrue(context.Controller.Enabled(typeof(PrimeNumbersCommand)));
                var feedback = new FeedbackToApplication() { Type = FeedbackType.DisableCommand, CommandType = typeof(PrimeNumbersCommand) };
                context.Context.Report(feedback);
                Thread.Sleep(ApplicationCommandControllerTestExecutionContext.MillisecondsToWaitForFeedbackToReturn);
                Assert.IsFalse(context.Controller.Enabled(typeof(PrimeNumbersCommand)));
            }
        }
    }
}