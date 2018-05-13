using System;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Basic;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application {
    public class FeedbackToApplication : IFeedbackToApplication {
        public FeedbackType Type { get; set; }
        public string Message { get; set; }
        public string Guid { get; set; }
        public Type CommandType { get; set; }
        public DateTime CreatedAt { get; set; }
        public long SequenceNumber { get; set; }

        public FeedbackToApplication() {
            Type = FeedbackType.MessageOfNoImportance;
            Message = "";
            Guid = System.Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            SequenceNumber = GlobalSequenceNumberGenerator.NextValue;
        }

        public FeedbackToApplication(string message, bool ofNoImportance) : this() {
            Type = ofNoImportance ? FeedbackType.MessageOfNoImportance : FeedbackType.ImportantMessage;
            Message = message;
        }
    }
}