using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application {
    public interface IApplicationCommand {
        bool MakeLogEntries { get; }
        string Name { get; }

        bool CanExecute();
        Task Execute(IApplicationCommandExecutionContext context);
    }
}
