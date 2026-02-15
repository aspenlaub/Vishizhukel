using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;

public class ApplicationCommandController(ISimpleLogger simpleLogger, Func<IFeedbackToApplication, Task> handleApplicationFeedbackAsync)
    : IApplicationCommandController, IApplicationCommandExecutionContext {
    protected List<IApplicationCommand> Commands = new();
    protected int MainThreadId = Thread.CurrentThread.ManagedThreadId;
    protected Func<IFeedbackToApplication, Task> HandleApplicationFeedbackAsync = handleApplicationFeedbackAsync;
    protected Dictionary<Type, int> EnableRequests = new(), DisableRequests = new();
    protected Dictionary<Type, bool> DefaultEnabled = new();
    protected DateTime DoNotReportMessagesOfNoImportanceUntil = DateTime.Now;
    protected bool IgnoringOfMessagesWasReported;
    protected static readonly int ReportMessagesOfNoImportanceEveryHowManyMilliseconds = 100;
    protected ISimpleLogger SimpleLogger = simpleLogger;

    protected async Task DisableCommandRunTaskAndEnableCommandAsync(IApplicationCommand command) {
        await DisableCommandAsync(command.GetType());
        await command.ExecuteAsync(this);
        await EnableCommandAsync(command.GetType());
    }

    public void AddCommand(IApplicationCommand command, bool defaultEnabled) {
        Commands.Add(command);
        Type commandType = command.GetType();
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
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ExecuteAsync)))) {
            IApplicationCommand command = Commands.FirstOrDefault(x => x.GetType() == commandType);
            if (command == null) {
                await HandleApplicationFeedbackAsync(new FeedbackToApplication { Type = FeedbackType.UnknownCommand, CommandType = commandType });
                return;
            }

            if (!await EnabledAsync(commandType)) {
                await HandleApplicationFeedbackAsync(new FeedbackToApplication { Type = FeedbackType.CommandIsDisabled, CommandType = commandType });
                return;
            }

            if (command.MakeLogEntries) {
                await HandleApplicationFeedbackAsync(new FeedbackToApplication { Type = FeedbackType.LogInformation, Message = string.Format(Properties.Resources.ExecutingCommand, command.Name) });
            }
            await DisableCommandRunTaskAndEnableCommandAsync(command);
            if (command.MakeLogEntries) {
                await HandleApplicationFeedbackAsync(new FeedbackToApplication { Type = FeedbackType.LogInformation, Message = string.Format(Properties.Resources.ExecutedCommand, command.Name) });
            }
            await HandleApplicationFeedbackAsync(new FeedbackToApplication { Type = FeedbackType.CommandExecutionCompleted, CommandType = command.GetType() });
        }
    }

    public async Task ReportAsync(IFeedbackToApplication feedback) {
        switch (feedback.Type) {
            case FeedbackType.EnableCommand: {
                await EnableCommandAsync(feedback.CommandType);
            } break;
            case FeedbackType.DisableCommand: {
                await DisableCommandAsync(feedback.CommandType);
            } break;
            case FeedbackType.MessageOfNoImportance: {
                await ReportAsync(feedback.Message, true);
            } break;
            case FeedbackType.ImportantMessage:
            case FeedbackType.MessagesOfNoImportanceWereIgnored:
            case FeedbackType.UnknownCommand:
            case FeedbackType.CommandIsDisabled:
            case FeedbackType.CommandExecutionCompleted:
            case FeedbackType.CommandExecutionCompletedWithMessage:
            case FeedbackType.CommandsEnabledOrDisabled:
            case FeedbackType.LogInformation:
            case FeedbackType.LogWarning:
            case FeedbackType.LogError:
            default: {
                await HandleApplicationFeedbackAsync(feedback);
            } break;
        }
    }

    public async Task ReportAsync(string message, bool ofNoImportance) {
        if (await IgnoreMessagesOfNoImportanceAsync(message, ofNoImportance)) { return; }

        await HandleApplicationFeedbackAsync(new FeedbackToApplication(message, ofNoImportance));
    }

    public async Task ReportExecutionResultAsync(Type commandType, bool success, string errorMessage) {
        string message = success ? "" : errorMessage;
        await HandleApplicationFeedbackAsync(new FeedbackToApplication { Type = FeedbackType.CommandExecutionCompletedWithMessage, Message = message, CommandType = commandType });
    }

    protected async Task<bool> IgnoreMessagesOfNoImportanceAsync(string message, bool ofNoImportance) {
        if (!ofNoImportance) { return false; }

        if (DoNotReportMessagesOfNoImportanceUntil > DateTime.Now) {
            if (IgnoringOfMessagesWasReported) { return true; }

            await HandleApplicationFeedbackAsync(new FeedbackToApplication { Type = FeedbackType.MessagesOfNoImportanceWereIgnored, Message = "..." });
            IgnoringOfMessagesWasReported = true;
            return true;
        }

        DoNotReportMessagesOfNoImportanceUntil = DateTime.Now.AddMilliseconds(ReportMessagesOfNoImportanceEveryHowManyMilliseconds);
        IgnoringOfMessagesWasReported = false;
        return false;
    }

    public async Task EnableCommandAsync(Type commandType) {
        await EnableOrDisableCommandAsync(commandType, true);
    }

    public async Task DisableCommandAsync(Type commandType) {
        await EnableOrDisableCommandAsync(commandType, false);
    }

    private async Task EnableOrDisableCommandAsync(Type commandType, bool enable) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(EnableOrDisableCommandAsync)))) {
            bool wasEnabled = await EnabledAsync(commandType);
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

            await HandleApplicationFeedbackAsync(new FeedbackToApplication { Type = FeedbackType.CommandsEnabledOrDisabled });
        }
    }

    public async Task<bool> EnabledAsync(Type commandType) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(EnabledAsync)))) {
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
    }
}