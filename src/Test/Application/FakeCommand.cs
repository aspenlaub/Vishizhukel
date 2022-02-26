using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    public class FakeCommand : IApplicationCommand {
        public bool MakeLogEntries => true;
        private readonly bool CanExecute;
        protected IApplicationCommandController Controller;
        public bool WasExecuted { get; private set; }

        public FakeCommand(bool canExecute, IApplicationCommandController controller) {
            CanExecute = canExecute;
            Controller = controller;
            WasExecuted = false;
        }

        public string Name => Properties.Resources.FakeCommandName;

        public async Task<bool> CanExecuteAsync() {
            return await Task.FromResult(CanExecute);
        }

        public async Task ExecuteAsync(IApplicationCommandExecutionContext context) {
            if (!CanExecute) {
                throw new NotImplementedException("Do not know how to execute a command that cannot be executed");
            }
            if (await Controller.EnabledAsync(GetType())) {
                throw new NotImplementedException("Command should be disabled while executing");
            }

            WasExecuted = true;
        }
    }
}
