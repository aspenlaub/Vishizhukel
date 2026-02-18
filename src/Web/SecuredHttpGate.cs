using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Skladasu.Entities;
using Aspenlaub.Net.GitHub.CSharp.Skladasu.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web;

public class SecuredHttpGate(IHttpGate httpGate, ISecuredHttpGateSettings securedHttpGateSettings,
        IFolderResolver folderResolver, IStringCrypter stringCrypter) : ISecuredHttpGate {
    public async Task<HtmlValidationResult> IsHtmlMarkupValidAsync(string markup) {
        string markupFileName = "";
        if (markup.Length > 10000) {
            var errorsAndInfos = new ErrorsAndInfos();
            string folder = (await folderResolver.ResolveAsync(securedHttpGateSettings.LocalhostTempPath, errorsAndInfos)).FullName;
            if (errorsAndInfos.AnyErrors()) {
                return new HtmlValidationResult { Success = false, ErrorMessage = errorsAndInfos.ErrorsToString() };
            }
            string shortFileName = "Markup" + markup.GetHashCode() + ".xml";
            markupFileName = folder + "\\" + shortFileName;
            await File.WriteAllTextAsync(markupFileName, markup, Encoding.UTF8);
            markup = securedHttpGateSettings.LocalhostTempPathUrl + shortFileName;
        }
        IAliceAndBob aliceAndBob = await CreateAliceAndBobAsync();
        HttpResponseMessage response = await httpGate.PostAsync(securedHttpGateSettings.ApiUrl, new Dictionary<string, string> { { "markup", markup }, { "alice", aliceAndBob.Alice }, { "bob", aliceAndBob.Bob }, { "command", "ValidateMarkup" } });
        if (markupFileName != "") {
            File.Delete(markupFileName);
        }
        if (response.StatusCode != HttpStatusCode.OK) { return new HtmlValidationResult { Success = false, ErrorMessage = "Service unavailable" }; }

        string json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<HtmlValidationResult>(json);
    }

    private async Task<IAliceAndBob> CreateAliceAndBobAsync() {
        var aliceAndBob = new AliceAndBob();
        var random = new Random();
        const string usables = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-";
        for (aliceAndBob.Bob = ""; aliceAndBob.Bob.Length < 16;) {
            aliceAndBob.Bob += usables[random.Next(0, usables.Length - 1)];
        }

        aliceAndBob.Alice = await stringCrypter.EncryptAsync(DateTime.Now.ToString("yyyy-MM-dd") + aliceAndBob.Bob);
        return aliceAndBob;
    }

    public async Task<bool> RegisterDefectAsync(string headline, string description, bool old) {
        IAliceAndBob aliceAndBob = await CreateAliceAndBobAsync();
        HttpResponseMessage response = await httpGate.PostAsync(securedHttpGateSettings.ApiUrl, new Dictionary<string, string> {
            { "headline", headline }, { "description", description }, { "alice", aliceAndBob.Alice }, { "bob", aliceAndBob.Bob }, { "command", old ? "RegisterOldDefect" : "RegisterDefect" }, { "source", "main" }
        });
        if (response.StatusCode != HttpStatusCode.OK) { return false; }

        string json = await response.Content.ReadAsStringAsync();
        HtmlValidationResult result = JsonSerializer.Deserialize<HtmlValidationResult>(json);
        return result?.Success == true;
    }
}