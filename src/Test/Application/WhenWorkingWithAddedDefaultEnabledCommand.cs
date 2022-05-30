using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithAddedDefaultEnabledCommand {
        protected ISimpleLogger SimpleLogger;

        [TestInitialize]
        public void Initialize() {
            var container = new ContainerBuilder().UsePegh("Vishizhukel", new DummyCsArgumentPrompter()).Build();
            SimpleLogger = container.Resolve<ISimpleLogger>();
        }

        [TestMethod]
        public async Task ThenCommandIsEnabled() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), true);
            Assert.IsTrue(await executionContext.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task ThenCommandCanBeDisabled() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), true);
            await executionContext.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsFalse(await executionContext.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task ThenCommandCanBeReEnabled() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), true);
            await executionContext.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            await executionContext.Controller.EnableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsTrue(await executionContext.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task ThenCommandCannotBeReEnabledIfDisabledTwice() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), true);
            await executionContext.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            await executionContext.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            await executionContext.Controller.EnableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsFalse(await executionContext.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task ThenCommandCanBeDisabledViaFeedback() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), true);
            Assert.IsTrue(await executionContext.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
            var feedback = new FeedbackToApplication() { Type = FeedbackType.DisableCommand, CommandType = typeof(PrimeNumbersCommand) };
            await executionContext.ExecutionContext.ReportAsync(feedback);
            Assert.IsFalse(await executionContext.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }
    }
}