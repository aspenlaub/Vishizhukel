using System.Collections.Generic;
using System.Text;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Core {
    public interface ITextFileWriter {
        void WriteAllLines(IFolder folder, string utf8FileName, List<string> lines, Encoding encoding);
        bool FileExistsAndIsIdentical(IFolder folder, string utf8FileName, List<string> lines, Encoding encoding);
    }
}