using System;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application {
    public interface IApplicationCommandExecutionContext {
        void Report(IFeedbackToApplication feedback);
        void Report(string message, bool ofNoImportance);
        // ReSharper disable once UnusedMember.Global
        void ReportExecutionResult(Type commandType, bool success, string errorMessage);
    }
}
