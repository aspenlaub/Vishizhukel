using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithCommandThatCanBeExecuted {
        [TestMethod]
        public async Task AddedEnabledCommandCanBeExecuted() {
            using (var context = new ApplicationCommandControllerTestExecutionContext()) {
                context.Controller.AddCommand(new PrimeNumbersCommand(), true);
                Assert.IsFalse(context.FeedbacksToApplication.Any(x => x.Type == FeedbackType.CommandExecutionCompleted));
                await context.Controller.Execute(typeof(PrimeNumbersCommand));
                await context.Controller.AwaitAllAsynchronousTasks();
                var feedbacksToApplication = context.GetFeedbacksToApplication();
                Assert.IsTrue(feedbacksToApplication.Any(x => x.Type == FeedbackType.CommandExecutionCompleted));
            }
        }

        [TestMethod]
        public void AddedDisabledCommandCanBeExecuted() {
            using (var context = new ApplicationCommandControllerTestExecutionContext()) {
                context.Controller.AddCommand(new PrimeNumbersCommand(), false);
                Assert.IsFalse(context.FeedbacksToApplication.Any(x => x.Type == FeedbackType.CommandExecutionCompleted));
                Assert.IsFalse(context.FeedbacksToApplication.Any(x => x.Type == FeedbackType.CommandIsDisabled));
                var task = context.Controller.Execute(typeof(PrimeNumbersCommand));
                task.Wait(ApplicationCommandControllerTestExecutionContext.MilliSecondsToWaitForFeedbackToReturn);
                Assert.IsTrue(context.FeedbacksToApplication.Any(x => x.Type == FeedbackType.CommandIsDisabled));
            }
        }

        [TestMethod]
        public void SomethingThatIsNotACommandCanBeExecutedButNotCompleted() {
            using (var context = new ApplicationCommandControllerTestExecutionContext()) {
                Assert.IsFalse(context.FeedbacksToApplication.Any(x => x.Type == FeedbackType.CommandExecutionCompleted));
                Assert.IsFalse(context.FeedbacksToApplication.Any(x => x.Type == FeedbackType.UnknownCommand));
                var task = context.Controller.Execute(GetType());
                task.Wait(ApplicationCommandControllerTestExecutionContext.MilliSecondsToWaitForFeedbackToReturn);
                Assert.IsFalse(context.FeedbacksToApplication.Any(x => x.Type == FeedbackType.CommandExecutionCompleted));
                Assert.IsTrue(context.FeedbacksToApplication.Any(x => x.Type == FeedbackType.UnknownCommand));
            }
        }
    }
}
