using System;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Basic;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;

public class FeedbackToApplication() : IFeedbackToApplication {
    public FeedbackType Type { get; set; } = FeedbackType.MessageOfNoImportance;
    public string Message { get; set; } = "";
    public string Guid { get; set; } = System.Guid.NewGuid().ToString();
    public Type CommandType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public long SequenceNumber { get; set; } = GlobalSequenceNumberGenerator.NextValue;

    public FeedbackToApplication(string message, bool ofNoImportance) : this() {
        Type = ofNoImportance ? FeedbackType.MessageOfNoImportance : FeedbackType.ImportantMessage;
        Message = message;
    }
}