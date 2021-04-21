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
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable UnusedMember.Global

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

            var securedHttpGateSettings = CreateSecuredHttpGateSettings(csArgumentPrompter);
            builder.RegisterInstance<ISecuredHttpGateSettings>(securedHttpGateSettings);

            builder.RegisterType<ApplicationLog>().As<IApplicationLog>();
            builder.RegisterType<HttpGate>().As<IHttpGate>();
            builder.RegisterType<SecuredHttpGate>().As<ISecuredHttpGate>();
            builder.RegisterType<TextFileWriter>().As<ITextFileWriter>();
            builder.RegisterType<WebFileSource>().As<IWebFileSource>();
            return builder;
        }

        public static IServiceCollection UseVishizhukelAndPegh(this IServiceCollection services, ICsArgumentPrompter csArgumentPrompter) {
            return UseVishizhukelAndPeghOptionallyDvin(services, csArgumentPrompter, false);
        }

        public static IServiceCollection UseVishizhukelDvinAndPegh(this IServiceCollection services, ICsArgumentPrompter csArgumentPrompter) {
            return UseVishizhukelAndPeghOptionallyDvin(services, csArgumentPrompter, true);
        }

        private static IServiceCollection UseVishizhukelAndPeghOptionallyDvin(this IServiceCollection services, ICsArgumentPrompter csArgumentPrompter, bool useDvin) {
            if (useDvin) {
                services.UseDvinAndPegh(csArgumentPrompter);
            } else {
                services.UsePegh(csArgumentPrompter);
            }

            var securedHttpGateSettings = CreateSecuredHttpGateSettings(csArgumentPrompter);
            services.AddSingleton<ISecuredHttpGateSettings>(securedHttpGateSettings);

            services.AddTransient<IApplicationLog, ApplicationLog>();
            services.AddTransient<IHttpGate, HttpGate>();
            services.AddTransient<ISecuredHttpGate, SecuredHttpGate>();
            services.AddTransient<ITextFileWriter, TextFileWriter>();
            services.AddTransient<IWebFileSource, WebFileSource>();
            return services;
        }

        private static SecuredHttpGateSettings CreateSecuredHttpGateSettings(ICsArgumentPrompter csArgumentPrompter) {
            var peghContainer = new ContainerBuilder().UsePegh(csArgumentPrompter).Build();
            var repository = peghContainer.Resolve<ISecretRepository>();
            var securedHttpGateSettingsSecret = new SecretSecuredHttpGateSettings();
            var errorsAndInfos = new ErrorsAndInfos();
            var securedHttpGateSettings = repository.GetAsync(securedHttpGateSettingsSecret, errorsAndInfos).Result;
            if (errorsAndInfos.AnyErrors()) {
                throw new Exception(errorsAndInfos.ErrorsToString());
            }

            return securedHttpGateSettings;
        }
    }
}
