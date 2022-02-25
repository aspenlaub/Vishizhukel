using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web {
    public class HttpGate : IHttpGate {
        private HttpClient Client { get; } = new();
        private const string LocalhostUrl = "http://localhost/";
        private const string AspenlaubUrl = "https://github.com/aspenlaub";

        public async Task<HttpResponseMessage> GetAsync(Uri uri) {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            try {
                return await Client.SendAsync(request);
            } catch {
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> variables) {
            var encodedVariables = new FormUrlEncodedContent(variables);
            try {
                return await Client.PostAsync(url, encodedVariables);
            } catch {
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
        }

        public async Task<bool> IsLocalHostAvailableAsync() {
            var response = await GetAsync(new Uri(LocalhostUrl));
            return response.StatusCode == HttpStatusCode.OK && response.Headers.Server.Any(s => s.Product?.Name.Contains("Apache") == true);
        }

        public async Task<bool> AreWeOnlineAsync() {
            var response = await GetAsync(new Uri(AspenlaubUrl));
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
