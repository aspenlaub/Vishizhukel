using System;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web {
    public class ExceptionSaverAndSender {
        public static void SaveUnhandledException(IFolder exceptionLogFolder, Exception exception, string source) {
            ExceptionSaver.SaveUnhandledException(exceptionLogFolder, exception, source, e => {
                var repository = new SecretRepository(new ComponentProvider());
                var securedHttpGateSettingsSecret = new SecretSecuredHttpGateSettings();
                var errorsAndInfos = new ErrorsAndInfos();
                var securedHttpGateSettings = repository.GetAsync(securedHttpGateSettingsSecret, errorsAndInfos).Result;

                ISecuredHttpGate gate = new SecuredHttpGate(new HttpGate(), securedHttpGateSettings);
                gate.RegisterDefectAsync(e.ExceptionName, e.FileContents, false).Wait();
            });
        }
    }
}
