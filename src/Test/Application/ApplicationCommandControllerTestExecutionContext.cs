using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    internal class ApplicationCommandControllerTestExecutionContext {
        internal IApplicationCommandController Controller { get; }
        internal IApplicationCommandExecutionContext ExecutionContext { get; }
        internal List<IFeedbackToApplication> FeedbacksToApplication { get; set; }
        internal object FeedbacksToApplicationLock;
        internal bool CommandsEnabledOrDisabledWasReported { get; set; }

        internal ApplicationCommandControllerTestExecutionContext() {
            var controller = new ApplicationCommandController(RecordApplicationFeedbackAsync);
            Controller = controller;
            ExecutionContext = controller;
            FeedbacksToApplication = new List<IFeedbackToApplication>();
            FeedbacksToApplicationLock = new object();
        }

        internal async Task RecordApplicationFeedbackAsync(IFeedbackToApplication feedback) {
            if (feedback.Type == FeedbackType.CommandsEnabledOrDisabled) {
                CommandsEnabledOrDisabledWasReported = true;
            }
            lock(FeedbacksToApplicationLock) {
                FeedbacksToApplication.Add(feedback);
            }

            await Task.CompletedTask;
        }

        internal List<IFeedbackToApplication> GetFeedbacksToApplication() {
            lock(FeedbacksToApplicationLock) {
                return new List<IFeedbackToApplication>(FeedbacksToApplication);
            }
        }
    }
}
