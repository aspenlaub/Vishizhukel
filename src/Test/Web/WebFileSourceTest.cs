using System;
using System.IO;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Web {
    [TestClass]
    public class WebFileSourceTest {
        private const string ReadMeUrl = "https://raw.githubusercontent.com/aspenlaub/Vishizhukel/master/build.cmd";
        private const string WrongReadMeUrl = "https://raw.githubusercontent.com/aspenlaub/Vishizhukel/master/duilb.cmd";
        internal const string ReadMeShortFileName = "README.md";

        private void CanUpdateLocalFileAcceptingOrIgnoringResult(WebFileSourceTestExecutionContext context, bool ignoreResult, out bool fileExistedUpfront, out bool upToDate) {
            fileExistedUpfront = File.Exists(context.LocalFileName);
            upToDate = false;
            if (ignoreResult) {
                context.WebFileSource.TryAndUpdateLocalCopyOfWebFile(ReadMeUrl, context.LocalFileName);
            } else {
                context.WebFileSource.TryAndUpdateLocalCopyOfWebFile(ReadMeUrl, context.LocalFileName, out upToDate);
            }
        }

        [TestMethod]
        public void CanUpdateLocalFile() {
            using var context = new WebFileSourceTestExecutionContext();
            CanUpdateLocalFileAcceptingOrIgnoringResult(context, false, out var fileExistedUpfront, out var upToDate);
            Assert.IsFalse(fileExistedUpfront);
            Assert.IsTrue(upToDate);
        }

        [TestMethod]
        public void CanUpdateLocalFileIgnoringTheResult() {
            using var context = new WebFileSourceTestExecutionContext();
            CanUpdateLocalFileAcceptingOrIgnoringResult(context, true, out var fileExistedUpfront, out var upToDate);
            Assert.IsFalse(fileExistedUpfront);
            Assert.IsFalse(upToDate);
        }

        [TestMethod]
        public void CannotUpdateLocalFileIfUrlIsInValid() {
            using var context = new WebFileSourceTestExecutionContext();
            Assert.IsFalse(File.Exists(context.LocalFileName));
            context.WebFileSource.TryAndUpdateLocalCopyOfWebFile(WrongReadMeUrl, context.LocalFileName, out var upToDate);
            Assert.IsFalse(upToDate);
        }
    }

    internal class WebFileSourceTestExecutionContext : IDisposable {
        internal IWebFileSource WebFileSource { get; }
        internal IFolder LocalFolder => new Folder(Path.GetTempPath()).SubFolder("AspenlaubTemp").SubFolder(nameof(WebFileSourceTest));
        internal string LocalFileName => LocalFolder.FullName + '\\' + WebFileSourceTest.ReadMeShortFileName;

        public WebFileSourceTestExecutionContext() {
            var container = new ContainerBuilder().UseVishizhukelAndPegh(new DummyCsArgumentPrompter()).Build();
            WebFileSource = container.Resolve<IWebFileSource>();
            Cleanup();
            LocalFolder.CreateIfNecessary();
        }

        protected void DeleteLocalFile() {
            if (!File.Exists(LocalFileName)) { return; }

            File.Delete(LocalFileName);
        }

        public void Cleanup() {
            DeleteLocalFile();
        }

        public void Dispose() {
            Cleanup();
            var folderDeleter = new FolderDeleter();
            folderDeleter.DeleteFolder(LocalFolder);
        }
    }
}