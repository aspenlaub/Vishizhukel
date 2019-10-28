using System;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application {
    public interface IFeedbackToApplication {
        FeedbackType Type { get; set; }
        string Guid { get; set; }
        string Message { get; set; }
        Type CommandType { get; set; }
        DateTime CreatedAt { get; set; }
        long SequenceNumber { get; set; }
    }
}
