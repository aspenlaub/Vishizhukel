using System;
using System.IO;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data {
    [TestClass]
    public class ApplicationDataFolderTest {
        protected string AppName, SubFolderShortName;
        protected IFolder ApplicationDataFolder;
        protected ApplicationFolders ApplicationFolders;
        protected IFolderDeleter FolderDeleter;
        protected IFolder RootFolder;

        [TestInitialize]
        public void Initialize() {
            AppName = GetType().Name;
            Assert.IsTrue(AppName.Length > 10);
            ApplicationDataFolder = new Folder(Path.GetTempPath());
            ApplicationDataFolder.CreateIfNecessary();
            ApplicationFolders = new ApplicationFolders(AppName, ApplicationDataFolder.FullName);
            RootFolder = ApplicationDataFolder.SubFolder(AppName);
            SubFolderShortName = "Data";
            FolderDeleter = new FolderDeleter();
        }

        [TestCleanup]
        public void Cleanup() {
            if (ApplicationDataFolder.Exists()) { return; }

            var deleter = new FolderDeleter();
            deleter.DeleteFolder(ApplicationDataFolder);
        }

        private void DeleteRootFolder(bool ifExists) {
            Assert.IsTrue(RootFolder.FullName.Length > 20);
            if (ifExists && !Directory.Exists(RootFolder.FullName)) { return; }

            Assert.IsTrue(Directory.Exists(RootFolder.FullName));
            Assert.IsTrue(FolderDeleter.CanDeleteFolder(RootFolder));
            FolderDeleter.DeleteFolder(RootFolder);
            Assert.IsFalse(Directory.Exists(RootFolder.FullName));
        }

        [TestMethod]
        public void CanGetApplicationFolder() {
            DeleteRootFolder(true);
            Assert.IsTrue(!Directory.Exists(RootFolder.FullName));
            var applicationFolder = ApplicationFolders.ApplicationFolder(SubFolderShortName);
            var subFolder = RootFolder.FullName + '\\' + SubFolderShortName;
            Assert.AreEqual(subFolder, applicationFolder.FullName);
            Assert.IsTrue(Directory.Exists(subFolder));
            DeleteRootFolder(false);
        }

        [TestMethod]
        public void CanGetEnvironmentFolder() {
            DeleteRootFolder(true);
            var applicationFolder = ApplicationFolders.ApplicationFolder(EnvironmentType.Qualification, SubFolderShortName);
            var subFolder = RootFolder.FullName + @"\Qualification\" + SubFolderShortName;
            Assert.AreEqual(subFolder, applicationFolder.FullName);
            DeleteRootFolder(false);
        }

        [TestMethod]
        public async Task ApplicationDataFolderIsSet() {
            var repository = new SecretRepository(new ComponentProvider());
            var applicationDataFolderSecret = new SecretApplicationDataFolders();
            var errorsAndInfos = new ErrorsAndInfos();
            var applicationDataFolders = await repository.GetAsync(applicationDataFolderSecret, errorsAndInfos);
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));
            var folderOnThisMachine = applicationDataFolders.FolderOnThisMachine();
            Assert.AreEqual(Environment.MachineName.ToLower(), folderOnThisMachine.Machine.ToLower());
        }
    }
}
