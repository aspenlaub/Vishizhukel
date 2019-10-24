using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Core;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core {
    public class TextFileWriter : ITextFileWriter {

        public void WriteAllLines(IFolder folder, string utf8FileName, List<string> lines, Encoding encoding) {
            var fileName = ReplaceInvalidCharacters(utf8FileName);
            if (folder.FullName.Length < 5) { return; }

            if (FileExistsAndIsIdentical(folder, fileName, lines, encoding)) { return; }

            CreateBackupFile(folder, fileName);
            var fullFileName = folder.FullName + '\\' + fileName;
            File.WriteAllLines(fullFileName, lines, encoding);
        }

        public bool FileExistsAndIsIdentical(IFolder folder, string utf8FileName, List<string> lines, Encoding encoding) {
            var fileName = ReplaceInvalidCharacters(utf8FileName);
            var fullFileName = folder.FullName + '\\' + fileName;
            if (!File.Exists(fullFileName)) { return false; }

            var existingLines = File.ReadAllLines(fullFileName, encoding).ToList();
            return existingLines.SequenceEqual(lines);
        }

        protected void CreateBackupFile(IFolder folder, string fileName) {
            if (!File.Exists(folder.FullName + '\\' + fileName)) { return; }
            if (!fileName.Contains('.')) { return; }

            var fileNameWithoutExtension = fileName.Substring(0, fileName.LastIndexOf('.') + 1);
            for (uint n = 1; n < 1000; n ++) {
                var suffix = n.ToString("D3");
                if (File.Exists(folder.FullName + '\\' + fileNameWithoutExtension + suffix)) {
                    continue;
                }

                File.Copy(folder.FullName + '\\' + fileName, folder.FullName + '\\' + fileNameWithoutExtension + suffix);
                return;
            }
        }

        private static string ReplaceInvalidCharacters(string utf8FileName) {
            return Path.GetInvalidFileNameChars().Aggregate(utf8FileName, (current, c) => current.Replace(c, '_'));
        }
    }
}
