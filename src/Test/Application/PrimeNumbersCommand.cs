using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    public class PrimeNumbersCommand : IApplicationCommand {
        public bool MakeLogEntries => true;
        public string Name => Properties.Resources.PrimeNumbersCommandName;

        public async Task<bool> CanExecuteAsync() {
            return await Task.FromResult(true);
        }

        public async Task ExecuteAsync(IApplicationCommandExecutionContext context) {
            context.Report("Starting calculating prime numbers, disable command for the moment", false);
            context.Report(new FeedbackToApplication() { Type = FeedbackType.DisableCommand, CommandType = GetType() });
            var task = CalculatePrimeNumbers(true, context);
            context.Report("Awaiting the prime numbers", false);
            await task;
            context.Report("Enable prime number command", false);
            context.Report(new FeedbackToApplication() { Type = FeedbackType.EnableCommand, CommandType = GetType() });
        }

        private static async Task CalculatePrimeNumbers(bool calledFromMainThread, IApplicationCommandExecutionContext context) {
            uint n = 1;
            do {
                n++;
                var nCopy = n;
                var isPrime = await Task.Run(() => IsPrime(nCopy)).ConfigureAwait(calledFromMainThread);
                if (!isPrime) { continue; }

                context.Report($"{n} is a prime number", true);
            } while (n < 100000);
        }

        private static bool IsPrime(uint n) {
            for (var i = 2; i * 2 <= n; i++) {
                if (n % i == 0) { return false; }
            }

            return true;
        }
    }
}
