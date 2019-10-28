using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Core;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Core {
    [TestClass]
    public class TextFileWriterTest {
        internal bool AreTheseFilesInTheTestFolder(TextFileWriterTestExecutionContext context, List<string> shortFileNames) {
            var existingFileNames = Directory.GetFiles(context.TestFolder.FullName, "*.*").OrderBy(x => x).ToList();
            return existingFileNames.SequenceEqual(shortFileNames.Select(x => context.TestFolder.FullName + '\\' + x));
        }

        [TestMethod]
        public void CanWriteAFile() {
            using var context = new TextFileWriterTestExecutionContext();
            context.TextFileWriter.WriteAllLines(context.TestFolder, context.TestFileName, context.Lines, Encoding.UTF8);
            Assert.IsTrue(File.Exists(context.TestFolder.FullName + '\\' + context.TestFileName));
            Assert.IsTrue(AreTheseFilesInTheTestFolder(context, new List<string> { context.TestFileName }));
            Assert.IsFalse(AreTheseFilesInTheTestFolder(context, new List<string> { context.TestFileName, context.TestFileName }));
            Assert.IsFalse(AreTheseFilesInTheTestFolder(context, new List<string> { "", context.TestFileName }));
            Assert.IsTrue(context.TextFileWriter.FileExistsAndIsIdentical(context.TestFolder, context.TestFileName, context.Lines, Encoding.UTF8));
        }

        [TestMethod]
        public void OverwriteFileWithNewContentsCreatesBackupFile() {
            using var context = new TextFileWriterTestExecutionContext();
            context.TextFileWriter.WriteAllLines(context.TestFolder, context.TestFileName, context.Lines, Encoding.UTF8);
            Assert.IsTrue(AreTheseFilesInTheTestFolder(context, new List<string> { context.TestFileName }));
            Assert.IsTrue(context.TextFileWriter.FileExistsAndIsIdentical(context.TestFolder, context.TestFileName, context.Lines, Encoding.UTF8));
            Assert.IsFalse(context.TextFileWriter.FileExistsAndIsIdentical(context.TestFolder, context.TestFileName, context.ChangedLines, Encoding.UTF8));
            context.TextFileWriter.WriteAllLines(context.TestFolder, context.TestFileName, context.ChangedLines, Encoding.UTF8);
            Assert.IsTrue(AreTheseFilesInTheTestFolder(context, new List<string> { context.TestFileBackup1Name, context.TestFileName }));
            Assert.IsTrue(context.TextFileWriter.FileExistsAndIsIdentical(context.TestFolder, context.TestFileName, context.ChangedLines, Encoding.UTF8));
            Assert.IsTrue(context.TextFileWriter.FileExistsAndIsIdentical(context.TestFolder, context.TestFileBackup1Name, context.Lines, Encoding.UTF8));
        }
    }

    internal class TextFileWriterTestExecutionContext : IDisposable {
        internal ITextFileWriter TextFileWriter { get; }
        internal IFolder TestFolder { get; }
        internal string TestFileName { get; }
        internal string TestFileBackup1Name { get; }
        internal List<string> Lines { get; }
        internal List<string> ChangedLines { get; }

        public TextFileWriterTestExecutionContext() {
            var container = new ContainerBuilder().UseVishizhukelAndPegh(new DummyCsArgumentPrompter()).Build();
            TextFileWriter = container.Resolve<ITextFileWriter>();
            var errorsAndInfos = new ErrorsAndInfos();
            TestFolder = new Folder(Path.GetTempPath()).SubFolder("AspenlaubTemp").SubFolder(nameof(TextFileWriterTest));
            Assert.IsFalse(errorsAndInfos.AnyErrors(), errorsAndInfos.ErrorsToString());
            TestFileName = "begin_end.txt";
            TestFileBackup1Name = "begin_end.001";
            Lines = new List<string> { "begin", "end" };
            ChangedLines = new List<string>() { "/* V=100 */", "begin", "end" };
            CleanUp();
            Directory.CreateDirectory(TestFolder.FullName);
        }

        public void CleanUp() {
            if (Directory.Exists(TestFolder.FullName)) {
                new FolderDeleter().DeleteFolder(new Folder(TestFolder.FullName));
            }
        }

        public void Dispose() {
            CleanUp();
        }
    }
}