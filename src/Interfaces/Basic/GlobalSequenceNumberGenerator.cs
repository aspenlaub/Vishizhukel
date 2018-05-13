namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Basic {
    public abstract class GlobalSequenceNumberGenerator {
        protected static long LastSequenceNumber;

        public static long NextValue { get { return ++LastSequenceNumber; } }
    }
}
