using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web {
    public interface IHttpGate {
        Task<bool> IsLocalHostAvailableAsync();
        Task<HttpResponseMessage> GetAsync(Uri uri);
        Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> variables);
        Task<bool> AreWeOnlineAsync();
    }
}
