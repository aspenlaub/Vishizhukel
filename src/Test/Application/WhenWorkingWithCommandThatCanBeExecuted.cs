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
            var container = new ContainerBuilder().UsePegh("Vishizhukel").Build();
            SimpleLogger = container.Resolve<ISimpleLogger>();
        }

        [TestMethod]
        public async Task AddedEnabledCommandCanBeExecuted() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), true);
            Assert.DoesNotContain(x => x.Type == FeedbackType.CommandExecutionCompleted, executionContext.FeedbacksToApplication);
            await executionContext.Controller.ExecuteAsync(typeof(PrimeNumbersCommand));
            var feedbacksToApplication = executionContext.GetFeedbacksToApplication();
            Assert.Contains(x => x.Type == FeedbackType.CommandExecutionCompleted, feedbacksToApplication);
        }

        [TestMethod]
        public async Task AddedDisabledCommandCanBeExecuted() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), false);
            Assert.DoesNotContain(x => x.Type == FeedbackType.CommandExecutionCompleted, executionContext.FeedbacksToApplication);
            Assert.DoesNotContain(x => x.Type == FeedbackType.CommandIsDisabled, executionContext.FeedbacksToApplication);
            await executionContext.Controller.ExecuteAsync(typeof(PrimeNumbersCommand));
            Assert.Contains(x => x.Type == FeedbackType.CommandIsDisabled, executionContext.FeedbacksToApplication);
        }

        [TestMethod]
        public async Task SomethingThatIsNotACommandCanBeExecutedButNotCompleted() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            Assert.DoesNotContain(x => x.Type == FeedbackType.CommandExecutionCompleted, executionContext.FeedbacksToApplication);
            Assert.DoesNotContain(x => x.Type == FeedbackType.UnknownCommand, executionContext.FeedbacksToApplication);
            await executionContext.Controller.ExecuteAsync(GetType());
            Assert.DoesNotContain(x => x.Type == FeedbackType.CommandExecutionCompleted, executionContext.FeedbacksToApplication);
            Assert.Contains(x => x.Type == FeedbackType.UnknownCommand, executionContext.FeedbacksToApplication);
        }
    }
}
