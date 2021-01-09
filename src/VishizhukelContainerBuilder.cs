using System;
using Aspenlaub.Net.GitHub.CSharp.Dvin.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Core;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel {
    public static class VishizhukelContainerBuilder {
        public static ContainerBuilder UseVishizhukelAndPegh(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter) {
            return UseVishizhukelAndPeghOptionallyDvin(builder, csArgumentPrompter, false);
        }

        public static ContainerBuilder UseVishizhukelDvinAndPegh(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter) {
            return UseVishizhukelAndPeghOptionallyDvin(builder, csArgumentPrompter, true);
        }

        private static ContainerBuilder UseVishizhukelAndPeghOptionallyDvin(ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter, bool useDvin) {
            if (useDvin) {
                builder.UseDvinAndPegh(csArgumentPrompter);
            } else {
                builder.UsePegh(csArgumentPrompter);
            }

            var peghContainer = new ContainerBuilder().UsePegh(csArgumentPrompter).Build();
            var repository = peghContainer.Resolve<ISecretRepository>();
            var securedHttpGateSettingsSecret = new SecretSecuredHttpGateSettings();
            var errorsAndInfos = new ErrorsAndInfos();
            var securedHttpGateSettings = repository.GetAsync(securedHttpGateSettingsSecret, errorsAndInfos).Result;
            if (errorsAndInfos.AnyErrors()) {
                throw new Exception(errorsAndInfos.ErrorsToString());
            }

            builder.RegisterInstance<ISecuredHttpGateSettings>(securedHttpGateSettings);

            builder.RegisterType<ApplicationLog>().As<IApplicationLog>();
            builder.RegisterType<HttpGate>().As<IHttpGate>();
            builder.RegisterType<SecuredHttpGate>().As<ISecuredHttpGate>();
            builder.RegisterType<TextFileWriter>().As<ITextFileWriter>();
            builder.RegisterType<WebFileSource>().As<IWebFileSource>();
            return builder;
        }
    }
}
