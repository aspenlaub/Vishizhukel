using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Basic;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Basic.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Application {
    // ReSharper disable once UnusedMember.Global
    public class LogEntry : IGuid, INotifyPropertyChanged, ILogEntry {
        [Key]
        public string Guid { get; set; }
        private DateTime PrivateCreatedAt;
        public DateTime CreatedAt { get => PrivateCreatedAt; set { PrivateCreatedAt = value; OnPropertyChanged(nameof(CreatedAt)); } }
        private LogEntryClass PrivateClass;
        public LogEntryClass Class { get => PrivateClass; set { PrivateClass = value; OnPropertyChanged(nameof(Class)); } }
        private string PrivateMessage;
        public string Message { get => PrivateMessage; set { PrivateMessage = value; OnPropertyChanged(nameof(Message)); } }
        private long PrivateSequenceNumber;
        public long SequenceNumber { get => PrivateSequenceNumber;
            set { PrivateSequenceNumber = value; OnPropertyChanged(nameof(SequenceNumber)); } }

        public LogEntry() {
            Guid = System.Guid.NewGuid().ToString();
            PrivateCreatedAt = DateTime.Now;
            PrivateClass = LogEntryClass.Information;
            PrivateMessage = "";
            SequenceNumber = GlobalSequenceNumberGenerator.NextValue;
        }

        protected void OnPropertyChanged(string propertyName) {
            // ReSharper disable once UseNullPropagation
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
