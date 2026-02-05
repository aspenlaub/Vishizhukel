using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenWorkingWithAddedDefaultDisabledCommand {
        protected ISimpleLogger SimpleLogger;

        [TestInitialize]
        public void Initialize() {
            var container = new ContainerBuilder().UsePegh("Vishizhukel").Build();
            SimpleLogger = container.Resolve<ISimpleLogger>();
        }

        [TestMethod]
        public async Task ThenCommandIsDisabled() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), false);
            Assert.IsFalse(await executionContext.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task WhenCommandCanBeReDisabled() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), false);
            await executionContext.Controller.EnableCommandAsync(typeof(PrimeNumbersCommand));
            await executionContext.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsFalse(await executionContext.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task ThenCommandCannotBeReDisabledIfEnabledTwice() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), false);
            await executionContext.Controller.EnableCommandAsync(typeof(PrimeNumbersCommand));
            await executionContext.Controller.EnableCommandAsync(typeof(PrimeNumbersCommand));
            await executionContext.Controller.DisableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsTrue(await executionContext.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task AddedDefaultDisabledCommandCanBeEnabledViaFeedback() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), false);
            Assert.IsFalse(await executionContext.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
            var feedback = new FeedbackToApplication() { Type = FeedbackType.EnableCommand, CommandType = typeof(PrimeNumbersCommand) };
            await executionContext.ExecutionContext.ReportAsync(feedback);
            Assert.IsTrue(await executionContext.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }

        [TestMethod]
        public async Task ThenCommandCanBeEnabled() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext(SimpleLogger);
            executionContext.Controller.AddCommand(new PrimeNumbersCommand(), false);
            await executionContext.Controller.EnableCommandAsync(typeof(PrimeNumbersCommand));
            Assert.IsTrue(await executionContext.Controller.EnabledAsync(typeof(PrimeNumbersCommand)));
        }
    }
}