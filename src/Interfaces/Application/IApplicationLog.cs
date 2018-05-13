using System.Collections.ObjectModel;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Basic.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application {
    public interface IApplicationLog {
        ObservableCollection<ILogEntry> LogEntries { get; }

        void Clear();
        void Add(ILogEntry entry);
    }
}
