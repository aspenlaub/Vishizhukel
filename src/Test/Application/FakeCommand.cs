using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    public class FakeCommand : IApplicationCommand {
        public bool MakeLogEntries { get { return true; } }
        private readonly bool vCanExecute;
        protected IApplicationCommandController Controller;
        public bool WasExecuted { get; private set; }

        public FakeCommand(bool canExecute, IApplicationCommandController controller) {
            vCanExecute = canExecute;
            Controller = controller;
            WasExecuted = false;
        }

        public string Name { get { return Properties.Resources.FakeCommandName; } }

        public bool CanExecute() {
            return vCanExecute;
        }

        public async Task Execute(IApplicationCommandExecutionContext context) {
            if (!vCanExecute) {
                throw new NotImplementedException("Do not know how to execute a command that cannot be executed");
            }
            if (Controller.Enabled(GetType())) {
                throw new NotImplementedException("Command should be disabled while executing");
            }

            WasExecuted = true;
            await Task.Delay(50);
        }
    }
}
