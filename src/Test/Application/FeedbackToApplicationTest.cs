using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class FeedbackToApplicationTest {
        [TestMethod]
        public void DefaultFeedbackIsMessageOfNoImportance() {
            var feedback = new FeedbackToApplication();
            Assert.AreEqual(FeedbackType.MessageOfNoImportance, feedback.Type);
            Assert.AreEqual("", feedback.Message);
        }

        [TestMethod]
        public void GuidIsDifferentForTwoNewPiecesOfFeedback() {
            IFeedbackToApplication feedback = new FeedbackToApplication(), moreFeedback = new FeedbackToApplication();
            Assert.AreNotEqual(feedback.Guid, moreFeedback.Guid);
        }

        protected static readonly string TestMessage = "This is not a test message.";

        [TestMethod]
        public void StringFeedbackIsMessageOfImportanceOrNot() {
            var feedback = new FeedbackToApplication(TestMessage, false);
            Assert.AreEqual(FeedbackType.ImportantMessage, feedback.Type);
            Assert.AreEqual(TestMessage, feedback.Message);
            feedback = new FeedbackToApplication(TestMessage, true);
            Assert.AreEqual(FeedbackType.MessageOfNoImportance, feedback.Type);
            Assert.AreEqual(TestMessage, feedback.Message);
        }
    }
}
