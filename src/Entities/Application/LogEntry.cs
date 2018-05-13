using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Basic;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Basic.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Application {
    public class LogEntry : IGuid, INotifyPropertyChanged, ILogEntry {
        [Key]
        public string Guid { get; set; }
        private DateTime vCreatedAt;
        public DateTime CreatedAt { get { return vCreatedAt; } set { vCreatedAt = value; OnPropertyChanged(nameof(CreatedAt)); } }
        private LogEntryClass vClass;
        public LogEntryClass Class { get { return vClass; } set { vClass = value; OnPropertyChanged(nameof(Class)); } }
        private string vMessage;
        public string Message { get { return vMessage; } set { vMessage = value; OnPropertyChanged(nameof(Message)); } }
        private long vSequenceNumber;
        public long SequenceNumber { get { return vSequenceNumber; } set { vSequenceNumber = value; OnPropertyChanged(nameof(SequenceNumber)); } }

        public LogEntry() {
            Guid = System.Guid.NewGuid().ToString();
            vCreatedAt = DateTime.Now;
            vClass = LogEntryClass.Information;
            vMessage = "";
            SequenceNumber = GlobalSequenceNumberGenerator.NextValue;
        }

        protected void OnPropertyChanged(string propertyName) {
            // ReSharper disable once UseNullPropagation
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
