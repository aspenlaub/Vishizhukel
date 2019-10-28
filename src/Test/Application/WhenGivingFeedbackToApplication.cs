using System;
using System.Collections.Generic;
using System.Threading;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenGivingFeedbackToApplication {
        protected static readonly string TestMessageOfNoImportance = "This is not a test message of no importance.";
        protected static readonly string ImportantTestMessage = "This is not a test message, but it is important.";

        [TestMethod]
        public void CanReportMessageOfNoImportance() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            var feedback = CreateFeedbackAndReport(context, FeedbackType.MessageOfNoImportance, TestMessageOfNoImportance);
            Thread.Sleep(ApplicationCommandControllerTestExecutionContext.MillisecondsToWaitForFeedbackToReturn);
            Assert.AreEqual(1, context.FeedbacksToApplication.Count);
            Assert.IsTrue(IsFeedbackEqualToRecordedFeedback(context, feedback, 0));
        }

        internal FeedbackToApplication CreateFeedbackAndReport(ApplicationCommandControllerTestExecutionContext context, FeedbackType type, string message) {
            var feedback = new FeedbackToApplication() { Type = type, Message = message };
            context.Context.Report(feedback);
            return feedback;
        }

        internal bool IsFeedbackEqualToRecordedFeedback(ApplicationCommandControllerTestExecutionContext context, IFeedbackToApplication feedback, int i) {
            return feedback.Type == context.FeedbacksToApplication[i].Type
                && feedback.Message == context.FeedbacksToApplication[i].Message
                && feedback.CommandType == context.FeedbacksToApplication[i].CommandType;
        }

        [TestMethod]
        public void CanReportImportantMessage() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            var feedback = CreateFeedbackAndReport(context, FeedbackType.ImportantMessage, ImportantTestMessage);
            Thread.Sleep(ApplicationCommandControllerTestExecutionContext.MillisecondsToWaitForFeedbackToReturn);
            Assert.AreEqual(1, context.FeedbacksToApplication.Count);
            Assert.IsTrue(IsFeedbackEqualToRecordedFeedback(context, feedback, 0));
        }

        [TestMethod]
        public void CanReportTwoMessagesWithDifferentImportance() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            var feedback = CreateFeedbackAndReport(context, FeedbackType.ImportantMessage, ImportantTestMessage);
            Thread.Sleep(ApplicationCommandControllerTestExecutionContext.MillisecondsToWaitForFeedbackToReturn);
            var anotherFeedback = CreateFeedbackAndReport(context, FeedbackType.MessageOfNoImportance, TestMessageOfNoImportance);
            Thread.Sleep(ApplicationCommandControllerTestExecutionContext.MillisecondsToWaitForFeedbackToReturn);
            Assert.AreEqual(2, context.FeedbacksToApplication.Count);
            Assert.IsTrue(IsFeedbackEqualToRecordedFeedback(context, feedback, 0));
            Assert.IsTrue(IsFeedbackEqualToRecordedFeedback(context, anotherFeedback, 1));
        }

        [TestMethod, Ignore]
        public void MassiveFeedbackOfNoImportanceIsReducedAndReportedEvery100Milliseconds() {
            using var context = new ApplicationCommandControllerTestExecutionContext();
            var feedbacks = new List<IFeedbackToApplication>();
            IFeedbackToApplication feedback;
            var feedbackUntil = DateTime.Now.AddMilliseconds(80);
            int i;
            for (i = 0; DateTime.Now < feedbackUntil; i++) {
                feedback = CreateFeedbackAndReport(context, FeedbackType.MessageOfNoImportance, i.ToString());
                feedbacks.Add(feedback);
            }

            Thread.Sleep(100);
            feedback = CreateFeedbackAndReport(context, FeedbackType.MessageOfNoImportance, "This is the end.");
            feedbacks.Add(feedback);
            Thread.Sleep(ApplicationCommandControllerTestExecutionContext.MillisecondsToWaitForFeedbackToReturn);
            Assert.IsTrue(feedbacks.Count > 2000);
            Assert.IsTrue(context.FeedbacksToApplication.Count == 3);
            Assert.IsTrue(IsFeedbackEqualToRecordedFeedback(context, feedbacks[0], 0));
            Assert.AreEqual(FeedbackType.MessagesOfNoImportanceWereIgnored, context.FeedbacksToApplication[1].Type);
            Assert.IsTrue(IsFeedbackEqualToRecordedFeedback(context, feedbacks[^1], 2));
        }
    }
}
