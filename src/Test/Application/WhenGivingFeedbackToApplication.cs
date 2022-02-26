using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Application {
    [TestClass]
    public class WhenGivingFeedbackToApplication {
        protected static readonly string TestMessageOfNoImportance = "This is not a test message of no importance.";
        protected static readonly string ImportantTestMessage = "This is not a test message, but it is important.";

        [TestMethod]
        public async Task CanReportMessageOfNoImportance() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext();
            var feedback = await CreateFeedbackAndReportAsync(executionContext, FeedbackType.MessageOfNoImportance, TestMessageOfNoImportance);
            Assert.AreEqual(1, executionContext.FeedbacksToApplication.Count);
            Assert.IsTrue(IsFeedbackEqualToRecordedFeedback(executionContext, feedback, 0));
        }

        internal async Task<FeedbackToApplication> CreateFeedbackAndReportAsync(ApplicationCommandControllerTestExecutionContext context, FeedbackType type, string message) {
            var feedback = new FeedbackToApplication() { Type = type, Message = message };
            await context.ExecutionContext.ReportAsync(feedback);
            return feedback;
        }

        internal bool IsFeedbackEqualToRecordedFeedback(ApplicationCommandControllerTestExecutionContext context, IFeedbackToApplication feedback, int i) {
            return feedback.Type == context.FeedbacksToApplication[i].Type
                && feedback.Message == context.FeedbacksToApplication[i].Message
                && feedback.CommandType == context.FeedbacksToApplication[i].CommandType;
        }

        [TestMethod]
        public async Task CanReportImportantMessage() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext();
            var feedback = await CreateFeedbackAndReportAsync(executionContext, FeedbackType.ImportantMessage, ImportantTestMessage);
            Assert.AreEqual(1, executionContext.FeedbacksToApplication.Count);
            Assert.IsTrue(IsFeedbackEqualToRecordedFeedback(executionContext, feedback, 0));
        }

        [TestMethod]
        public async Task CanReportTwoMessagesWithDifferentImportance() {
            var executionContext = new ApplicationCommandControllerTestExecutionContext();
            var feedback = await CreateFeedbackAndReportAsync(executionContext, FeedbackType.ImportantMessage, ImportantTestMessage);
            var anotherFeedback = await CreateFeedbackAndReportAsync(executionContext, FeedbackType.MessageOfNoImportance, TestMessageOfNoImportance);
            Assert.AreEqual(2, executionContext.FeedbacksToApplication.Count);
            Assert.IsTrue(IsFeedbackEqualToRecordedFeedback(executionContext, feedback, 0));
            Assert.IsTrue(IsFeedbackEqualToRecordedFeedback(executionContext, anotherFeedback, 1));
        }
    }
}
