using System;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Autofac;

// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web {
    public class ExceptionSaverAndSender {
        public static void SaveUnhandledException(IFolder exceptionLogFolder, Exception exception, string source) {
            ExceptionSaver.SaveUnhandledException(exceptionLogFolder, exception, source, e => {
                var container = new ContainerBuilder().UsePegh(new DummyCsArgumentPrompter()).Build();
                var repository = container.Resolve<ISecretRepository>();
                var securedHttpGateSettingsSecret = new SecretSecuredHttpGateSettings();
                var errorsAndInfos = new ErrorsAndInfos();
                var securedHttpGateSettings = repository.GetAsync(securedHttpGateSettingsSecret, errorsAndInfos).Result;

                ISecuredHttpGate gate = new SecuredHttpGate(new HttpGate(), securedHttpGateSettings, container.Resolve<IFolderResolver>(),
                    container.Resolve<IStringCrypter>());
                gate.RegisterDefectAsync(e.ExceptionName, e.FileContents, false).Wait();
            });
        }
    }
}
