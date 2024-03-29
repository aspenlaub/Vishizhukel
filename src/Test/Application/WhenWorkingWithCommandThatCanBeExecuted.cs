using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithCommandThatCanBeExecuted {
        protected ISimpleLogger SimpleLogger;

        [TestInitialize]
        public void Initialize() {
            var container = new ContainerBuilder().UsePegh("Vishizhukel", new DummyCsArgumentPrompter()).Build();
            SimpleLogger = container.Resolve<ISimpleLogger>();
        }

        [TestMethod]
        public async Task AddedEnabledCommandCanBeExecuted() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), true);
            Assert.IsFalse(executionContext.FeedbacksToApplication.Any(x => x.Type == FeedbackType.CommandExecutionCompleted));
            await executionContext.Controller.ExecuteAsync(typeof(PrimeNumbersCommand));
            var feedbacksToApplication = executionContext.GetFeedbacksToApplication();
            Assert.IsTrue(feedbacksToApplication.Any(x => x.Type == FeedbackType.CommandExecutionCompleted));
        }

        [TestMethod]
        public async Task AddedDisabledCommandCanBeExecuted() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), false);
            Assert.IsFalse(executionContext.FeedbacksToApplication.Any(x => x.Type == FeedbackType.CommandExecutionCompleted));
            Assert.IsFalse(executionContext.FeedbacksToApplication.Any(x => x.Type == FeedbackType.CommandIsDisabled));
            await executionContext.Controller.ExecuteAsync(typeof(PrimeNumbersCommand));
            Assert.IsTrue(executionContext.FeedbacksToApplication.Any(x => x.Type == FeedbackType.CommandIsDisabled));
        }

        [TestMethod]
        public async Task SomethingThatIsNotACommandCanBeExecutedButNotCompleted() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            Assert.IsFalse(executionContext.FeedbacksToApplication.Any(x => x.Type == FeedbackType.CommandExecutionCompleted));
            Assert.IsFalse(executionContext.FeedbacksToApplication.Any(x => x.Type == FeedbackType.UnknownCommand));
            await executionContext.Controller.ExecuteAsync(GetType());
            Assert.IsFalse(executionContext.FeedbacksToApplication.Any(x => x.Type == FeedbackType.CommandExecutionCompleted));
            Assert.IsTrue(executionContext.FeedbacksToApplication.Any(x => x.Type == FeedbackType.UnknownCommand));
        }
    }
}
