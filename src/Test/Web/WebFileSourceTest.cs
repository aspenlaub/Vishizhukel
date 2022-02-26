using System;
using System.IO;
using System.Threading.Tasks;
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

        private async Task<UpdateResult> CanUpdateLocalFileAcceptingOrIgnoringResultAsync(WebFileSourceTestExecutionContext context, bool ignoreResult) {
            var result = new UpdateResult {
                FileExistedUpfront = File.Exists(context.LocalFileName),
                UpToDate = false
            };
            if (ignoreResult) {
                await context.WebFileSource.TryAndUpdateLocalCopyOfWebFileAsync(ReadMeUrl, context.LocalFileName);
            } else {
                result.UpToDate = await context.WebFileSource.TryAndUpdateLocalCopyOfWebFileReturnUpToDateAsync(ReadMeUrl, context.LocalFileName);
            }

            return result;
        }

        [TestMethod]
        public async Task CanUpdateLocalFile() {
            using var executionContext = new WebFileSourceTestExecutionContext();
            var result = await CanUpdateLocalFileAcceptingOrIgnoringResultAsync(executionContext, false);
            Assert.IsFalse(result.FileExistedUpfront);
            Assert.IsTrue(result.UpToDate);
        }

        [TestMethod]
        public async Task CanUpdateLocalFileIgnoringTheResult() {
            using var executionContext = new WebFileSourceTestExecutionContext();
            var result = await CanUpdateLocalFileAcceptingOrIgnoringResultAsync(executionContext, true);
            Assert.IsFalse(result.FileExistedUpfront);
            Assert.IsFalse(result.UpToDate);
        }

        [TestMethod]
        public async Task CannotUpdateLocalFileIfUrlIsInValid() {
            using var executionContext = new WebFileSourceTestExecutionContext();
            Assert.IsFalse(File.Exists(executionContext.LocalFileName));
            var upToDate = await executionContext.WebFileSource.TryAndUpdateLocalCopyOfWebFileReturnUpToDateAsync(WrongReadMeUrl, executionContext.LocalFileName);
            Assert.IsFalse(upToDate);
        }
    }

    internal class WebFileSourceTestExecutionContext : IDisposable {
        internal IWebFileSource WebFileSource { get; }
        internal IFolder LocalFolder => new Folder(Path.GetTempPath()).SubFolder("AspenlaubTemp").SubFolder(nameof(WebFileSourceTest));
        internal string LocalFileName => LocalFolder.FullName + '\\' + WebFileSourceTest.ReadMeShortFileName;

        public WebFileSourceTestExecutionContext() {
            var container = new ContainerBuilder().UseVishizhukelAndPeghAsync(new DummyCsArgumentPrompter()).Result.Build();
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

    internal class UpdateResult {
        public bool FileExistedUpfront { get; set; }
        public bool UpToDate { get; set; }
    }
}