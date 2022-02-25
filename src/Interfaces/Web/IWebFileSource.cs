using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web {
    public interface IWebFileSource {
        /// <summary>Try to get a file from a url, update local file copy if necessary.</summary>
        Task TryAndUpdateLocalCopyOfWebFileAsync(string fileSourceUrl, string localCopyFileName);

        /// <summary>Try to get a file from a url, update local file copy if necessary, return if the copy is up-to-date.</summary>
        Task<bool> TryAndUpdateLocalCopyOfWebFileReturnUpToDateAsync(string fileSourceUrl, string localCopyFileName);
    }
}
