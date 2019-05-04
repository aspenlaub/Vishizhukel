using System.IO;
using System.Net;
using System.Text;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web {
    public class WebFileSource : IWebFileSource {
        /// <summary>Try to get a file from a url, update local file copy if necessary.</summary>
        public void TryAndUpdateLocalCopyOfWebFile(string fileSourceUrl, string localCopyFileName) {
            TryAndUpdateLocalCopyOfWebFile(fileSourceUrl, localCopyFileName, out _);
        }

        /// <summary>Try to get a file from a url, update local file copy if necessary, return if the copy is up-to-date.</summary>
        public void TryAndUpdateLocalCopyOfWebFile(string fileSourceUrl, string localCopyFileName, out bool upToDate) {
            upToDate = true;
            try {
                var webClient = new WebClient();
                var contents = "This is not the contents";
                using (var stream = webClient.OpenRead(fileSourceUrl)) {
                    if (stream != null) {
                        using (var sr = new StreamReader(stream, Encoding.UTF8)) {
                            contents = sr.ReadToEnd();
                        }
                    }
                }
                if (File.Exists(localCopyFileName) && contents == File.ReadAllText(localCopyFileName)) { return; }

                File.WriteAllText(localCopyFileName, contents, Encoding.UTF8);
            } catch {
                upToDate = false;
            }
        }
    }
}
