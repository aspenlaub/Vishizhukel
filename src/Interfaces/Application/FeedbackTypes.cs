namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application {
    public enum FeedbackType {
        ImportantMessage, MessageOfNoImportance, MessagesOfNoImportanceWereIgnored,
        EnableCommand, DisableCommand, UnknownCommand, CommandIsDisabled,
        CommandExecutionCompleted, CommandExecutionCompletedWithMessage,
        CommandsEnabledOrDisabled,
        LogInformation, LogWarning, LogError,
    }
}
