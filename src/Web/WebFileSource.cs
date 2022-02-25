using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web {
    public class WebFileSource : IWebFileSource {
        /// <summary>Try to get a file from a url, update local file copy if necessary.</summary>
        public async Task TryAndUpdateLocalCopyOfWebFileAsync(string fileSourceUrl, string localCopyFileName) {
            await TryAndUpdateLocalCopyOfWebFileReturnUpToDateAsync(fileSourceUrl, localCopyFileName);
        }

        /// <summary>Try to get a file from a url, update local file copy if necessary, return if the copy is up-to-date.</summary>
        public async Task<bool> TryAndUpdateLocalCopyOfWebFileReturnUpToDateAsync(string fileSourceUrl, string localCopyFileName) {
            if (localCopyFileName == null) {
                throw new ArgumentNullException(nameof(localCopyFileName));
            }

            try {
                var client = new HttpClient();
                string contents;
                using (var response = await client.GetAsync(fileSourceUrl)) {
                    if (response.StatusCode != HttpStatusCode.OK) { return false; }

                    using (var content = response.Content) {
                        contents = await content.ReadAsStringAsync();
                    }
                }
                if (File.Exists(localCopyFileName) && contents == await File.ReadAllTextAsync(localCopyFileName)) { return true; }

                await File.WriteAllTextAsync(localCopyFileName, contents, Encoding.UTF8);
                return true;
            } catch {
                return false;
            }
        }
    }
}
