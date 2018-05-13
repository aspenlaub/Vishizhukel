using System;
using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    internal class ApplicationCommandControllerTestExecutionContext : IDisposable {
        internal IApplicationCommandController Controller { get; }
        internal IApplicationCommandExecutionContext Context { get; private set; }
        internal List<IFeedbackToApplication> FeedbacksToApplication { get; set; }
        internal object FeedbacksToApplicationLock;
        internal bool CommandsEnabledOrDisabledWasReported { get; set; }

        internal static readonly int MilliSecondsToWaitForFeedbackToReturn = 5;

        internal ApplicationCommandControllerTestExecutionContext() {
            var controller = new ApplicationCommandController(RecordApplicationFeedback);
            Controller = controller;
            Context = controller;
            FeedbacksToApplication = new List<IFeedbackToApplication>();
            FeedbacksToApplicationLock = new object();
        }

        internal void RecordApplicationFeedback(IFeedbackToApplication feedback) {
            if (feedback.Type == FeedbackType.CommandsEnabledOrDisabled) {
                CommandsEnabledOrDisabledWasReported = true;
            }
            lock(FeedbacksToApplicationLock) {
                FeedbacksToApplication.Add(feedback);
            }
        }

        internal List<IFeedbackToApplication> GetFeedbacksToApplication() {
            lock(FeedbacksToApplicationLock) {
                return new List<IFeedbackToApplication>(FeedbacksToApplication);
            }
        }

        public void Dispose() {
            Controller.AwaitAllAsynchronousTasks().Wait();
        }
    }
}
