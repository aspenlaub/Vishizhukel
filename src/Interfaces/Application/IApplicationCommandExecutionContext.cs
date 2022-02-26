using System;
using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application {
    public interface IApplicationCommandExecutionContext {
        Task ReportAsync(IFeedbackToApplication feedback);
        Task ReportAsync(string message, bool ofNoImportance);
        // ReSharper disable once UnusedMember.Global
        Task ReportExecutionResultAsync(Type commandType, bool success, string errorMessage);
    }
}
