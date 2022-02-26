using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application {
    public class ApplicationCommandController : IApplicationCommandController, IApplicationCommandExecutionContext {
        protected List<IApplicationCommand> Commands;
        protected int MainThreadId;
        protected IProgress<IFeedbackToApplication> FeedbackToApplicationRegistry;
        protected Action<IFeedbackToApplication> ApplicationFeedbackHandler;
        protected Dictionary<Type, int> EnableRequests, DisableRequests;
        protected Dictionary<Type, bool> DefaultEnabled;
        protected DateTime DoNotReportMessagesOfNoImportanceUntil;
        protected bool IgnoringOfMessagesWasReported;
        protected List<Task> StartedAsynchronousTasks;
        protected static readonly int ReportMessagesOfNoImportanceEveryHowManyMilliseconds = 100;

        public ApplicationCommandController(Action<IFeedbackToApplication> applicationFeedbackHandler) {
            MainThreadId = Thread.CurrentThread.ManagedThreadId;
            Commands = new List<IApplicationCommand>();
            ApplicationFeedbackHandler = applicationFeedbackHandler;
            EnableRequests = new Dictionary<Type, int>();
            DisableRequests = new Dictionary<Type, int>();
            DefaultEnabled = new Dictionary<Type, bool>();
            DoNotReportMessagesOfNoImportanceUntil = DateTime.Now;
            IgnoringOfMessagesWasReported = false;
            StartedAsynchronousTasks = new List<Task>();
            var feedbackToApplicationRegistry = new Progress<IFeedbackToApplication>();
            feedbackToApplicationRegistry.ProgressChanged += ControllerApplicationFeedbackHandler;
            FeedbackToApplicationRegistry = feedbackToApplicationRegistry;
        }

        protected async Task RunTaskAsync(Task task) {
            StartedAsynchronousTasks.Add(task);
            await task;
        }

        protected async Task DisableCommandRunTaskAndEnableCommandAsync(IApplicationCommand command) {
            await DisableCommandAsync(command.GetType());
            await command.ExecuteAsync(this);
            await EnableCommandAsync(command.GetType());
        }

        protected async void ControllerApplicationFeedbackHandler(object sender, IFeedbackToApplication feedback) {
            switch (feedback.Type) {
                case FeedbackType.EnableCommand: {
                    await RunTaskAsync(EnableCommandAsync(feedback.CommandType));
                }
                break;
                case FeedbackType.DisableCommand: {
                    await RunTaskAsync(DisableCommandAsync(feedback.CommandType));
                }
                break;
                default: {
                    ApplicationFeedbackHandler(feedback);
                }
                break;
            }
        }

        public void AddCommand(IApplicationCommand command, bool defaultEnabled) {
            Commands.Add(command);
            var commandType = command.GetType();
            lock (EnableRequests) {
                EnableRequests[commandType] = 0;
            }
            lock (DisableRequests) {
                DisableRequests[commandType] = 0;
            }
            DefaultEnabled[commandType] = defaultEnabled;
        }

        public bool IsMainThread() {
            return Thread.CurrentThread.ManagedThreadId == MainThreadId;
        }

        public async Task ExecuteAsync(Type commandType) {
            var command = Commands.FirstOrDefault(x => x.GetType() == commandType);
            if (command == null) {
                FeedbackToApplicationRegistry.Report(new FeedbackToApplication() { Type = FeedbackType.UnknownCommand, CommandType = commandType });
                await RunTaskAsync(Task.Delay(50));
                return;
            }

            if (!await EnabledAsync(commandType)) {
                FeedbackToApplicationRegistry.Report(new FeedbackToApplication() { Type = FeedbackType.CommandIsDisabled, CommandType = commandType });
                await RunTaskAsync(Task.Delay(50));
                return;
            }

            if (command.MakeLogEntries) {
                FeedbackToApplicationRegistry.Report(new FeedbackToApplication() { Type = FeedbackType.LogInformation, Message = string.Format(Properties.Resources.ExecutingCommand, command.Name) });
            }
            await RunTaskAsync(DisableCommandRunTaskAndEnableCommandAsync(command));
            if (command.MakeLogEntries) {
                FeedbackToApplicationRegistry.Report(new FeedbackToApplication() { Type = FeedbackType.LogInformation, Message = string.Format(Properties.Resources.ExecutedCommand, command.Name) });
            }
            FeedbackToApplicationRegistry.Report(new FeedbackToApplication() { Type = FeedbackType.CommandExecutionCompleted, CommandType = command.GetType() });
        }

        public void Report(IFeedbackToApplication feedback) {
            if (feedback.Type == FeedbackType.MessageOfNoImportance) {
                Report(feedback.Message, true);
            } else {
                FeedbackToApplicationRegistry.Report(feedback);
            }
        }

        public void Report(string message, bool ofNoImportance) {
            if (IgnoreMessagesOfNoImportance(message, ofNoImportance)) { return; }

            FeedbackToApplicationRegistry.Report(new FeedbackToApplication(message, ofNoImportance));
        }

        public void ReportExecutionResult(Type commandType, bool success, string errorMessage) {
            var message = success ? "" : errorMessage;
            FeedbackToApplicationRegistry.Report(new FeedbackToApplication() { Type = FeedbackType.CommandExecutionCompletedWithMessage, Message = message, CommandType = commandType });
        }

        protected bool IgnoreMessagesOfNoImportance(string message, bool ofNoImportance) {
            if (!ofNoImportance) { return false; }

            if (DoNotReportMessagesOfNoImportanceUntil > DateTime.Now) {
                if (IgnoringOfMessagesWasReported) { return true; }

                FeedbackToApplicationRegistry.Report(new FeedbackToApplication() { Type = FeedbackType.MessagesOfNoImportanceWereIgnored, Message = "..." });
                IgnoringOfMessagesWasReported = true;
                return true;
            }

            DoNotReportMessagesOfNoImportanceUntil = DateTime.Now.AddMilliseconds(ReportMessagesOfNoImportanceEveryHowManyMilliseconds);
            IgnoringOfMessagesWasReported = false;
            return false;
        }

        public async Task EnableCommandAsync(Type commandType) {
            await RunTaskAsync(EnableOrDisableCommandAsync(commandType, true));
        }

        public async Task DisableCommandAsync(Type commandType) {
            await RunTaskAsync(EnableOrDisableCommandAsync(commandType, false));
        }

        private async Task EnableOrDisableCommandAsync(Type commandType, bool enable) {
            var wasEnabled = await EnabledAsync(commandType);
            lock (this) {
                if (enable) {
                    EnableRequests[commandType]++;
                } else {
                    DisableRequests[commandType]++;
                }
                if (DisableRequests[commandType] == EnableRequests[commandType]) {
                    DisableRequests[commandType] = 0;
                    EnableRequests[commandType] = 0;
                }
            }
            if (wasEnabled == await EnabledAsync(commandType)) { return; }

            FeedbackToApplicationRegistry.Report(new FeedbackToApplication() { Type = FeedbackType.CommandsEnabledOrDisabled });
            await RunTaskAsync(Task.Delay(50));
        }

        public async Task<bool> EnabledAsync(Type commandType) {
            bool enabled;
            IApplicationCommand command;
            lock (this) {
                if (!DefaultEnabled.Keys.Contains(commandType)) {
                    enabled = false;
                } else if (DefaultEnabled[commandType]) {
                    enabled = DisableRequests[commandType] == 0;
                } else {
                    enabled = EnableRequests[commandType] != 0;
                }
                if (!enabled) { return false; }

                command = Commands.FirstOrDefault(x => x.GetType() == commandType);
            }

            enabled = command != null && await command.CanExecuteAsync();
            return enabled;
        }

        public async Task AwaitAllAsynchronousTasksAsync() {
            if (StartedAsynchronousTasks.Any()) {
                FeedbackToApplicationRegistry.Report(new FeedbackToApplication() { Type = FeedbackType.LogInformation, Message = Properties.Resources.AwaitingAllAsynchronousTasks });
            }
            await Task.WhenAll(StartedAsynchronousTasks);
            StartedAsynchronousTasks.Clear();
        }
    }
}
