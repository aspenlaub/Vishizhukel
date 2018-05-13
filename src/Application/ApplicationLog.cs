using System.Collections.ObjectModel;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Basic.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application {
    public class ApplicationLog : IApplicationLog {
        public ObservableCollection<ILogEntry> LogEntries { get; }

        public ApplicationLog() {
            LogEntries = new ObservableCollection<ILogEntry>();
        }

        public void Clear() {
            LogEntries.Clear();
        }

        public void Add(ILogEntry entry) {
            LogEntries.Add(entry);
        }
    }
}
