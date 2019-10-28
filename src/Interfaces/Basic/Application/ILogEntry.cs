using System;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Basic.Application {
    public interface ILogEntry {
        LogEntryClass Class { get; set; }
        string Message { get; set; }
        DateTime CreatedAt { get; set; }
        long SequenceNumber { get; set; }
    }
}