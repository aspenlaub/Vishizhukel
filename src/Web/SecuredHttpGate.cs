using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Newtonsoft.Json;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web {
    public class SecuredHttpGate : ISecuredHttpGate {
        private readonly IHttpGate HttpGate;
        private readonly ISecuredHttpGateSettings SecuredHttpGateSettings;
        private readonly IFolderResolver FolderResolver;
        private readonly IStringCrypter StringCrypter;

        public SecuredHttpGate(IHttpGate httpGate, ISecuredHttpGateSettings securedHttpGateSettings, IFolderResolver folderResolver, IStringCrypter stringCrypter) {
            HttpGate = httpGate;
            SecuredHttpGateSettings = securedHttpGateSettings;
            FolderResolver = folderResolver;
            StringCrypter = stringCrypter;
        }

        public async Task<HtmlValidationResult> IsHtmlMarkupValidAsync(string markup) {
            var markupFileName = "";
            if (markup.Length > 10000) {
                var errorsAndInfos = new ErrorsAndInfos();
                var folder = (await FolderResolver.ResolveAsync(SecuredHttpGateSettings.LocalhostTempPath, errorsAndInfos)).FullName;
                if (errorsAndInfos.AnyErrors()) {
                    return new HtmlValidationResult { Success = false, ErrorMessage = errorsAndInfos.ErrorsToString() };
                }
                var shortFileName = "Markup" + markup.GetHashCode() + ".xml";
                markupFileName = folder + "\\" + shortFileName;
                await File.WriteAllTextAsync(markupFileName, markup, Encoding.UTF8);
                markup = SecuredHttpGateSettings.LocalhostTempPathUrl + shortFileName;
            }
            var aliceAndBob = await CreateAliceAndBobAsync();
            var response = await HttpGate.PostAsync(SecuredHttpGateSettings.ApiUrl, new Dictionary<string, string> { { "markup", markup }, { "alice", aliceAndBob.Alice }, { "bob", aliceAndBob.Bob }, { "command", "ValidateMarkup" } });
            if (markupFileName != "") {
                File.Delete(markupFileName);
            }
            if (response.StatusCode != HttpStatusCode.OK) { return new HtmlValidationResult { Success = false, ErrorMessage = "Service unavailable" }; }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HtmlValidationResult>(json);
        }

        private async Task<IAliceAndBob> CreateAliceAndBobAsync() {
            var aliceAndBob = new AliceAndBob();
            var random = new Random();
            const string usables = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-";
            for (aliceAndBob.Bob = ""; aliceAndBob.Bob.Length < 16;) {
                aliceAndBob.Bob = aliceAndBob.Bob + usables[random.Next(0, usables.Length - 1)];
            }

            aliceAndBob.Alice = await StringCrypter.EncryptAsync(DateTime.Now.ToString("yyyy-MM-dd") + aliceAndBob.Bob);
            return aliceAndBob;
        }

        public async Task<bool> RegisterDefectAsync(string headline, string description, bool old) {
            var aliceAndBob = await CreateAliceAndBobAsync();
            var response = await HttpGate.PostAsync(SecuredHttpGateSettings.ApiUrl, new Dictionary<string, string> {
                { "headline", headline }, { "description", description }, { "alice", aliceAndBob.Alice }, { "bob", aliceAndBob.Bob }, { "command", old ? "RegisterOldDefect" : "RegisterDefect" }, { "source", "main" }
            });
            if (response.StatusCode != HttpStatusCode.OK) { return false; }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<HtmlValidationResult>(json);
            return result?.Success == true;
        }
    }
}
