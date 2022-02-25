using System;
using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application {
    public interface IApplicationCommandController {
        void AddCommand(IApplicationCommand command, bool defaultEnabled);

        Task<bool> EnabledAsync(Type commandType);
        Task ExecuteAsync(Type commandType);

        Task EnableCommand(Type commandType);
        Task DisableCommand(Type commandType);

        bool IsMainThread();

        Task AwaitAllAsynchronousTasks();
    }
}
