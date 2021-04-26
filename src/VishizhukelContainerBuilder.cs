using System;
using System.Threading.Tasks;
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
        public static async Task<ContainerBuilder> UseVishizhukelAndPeghAsync(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter) {
            return await UseVishizhukelAndPeghOptionallyDvinAsync(builder, csArgumentPrompter, false);
        }

        public static async Task<ContainerBuilder> UseVishizhukelDvinAndPeghAsync(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter) {
            return await UseVishizhukelAndPeghOptionallyDvinAsync(builder, csArgumentPrompter, true);
        }

        private static async Task<ContainerBuilder> UseVishizhukelAndPeghOptionallyDvinAsync(ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter, bool useDvin) {
            if (useDvin) {
                builder.UseDvinAndPegh(csArgumentPrompter);
            } else {
                builder.UsePegh(csArgumentPrompter);
            }

            var securedHttpGateSettings = await CreateSecuredHttpGateSettingsAsync(csArgumentPrompter);
            builder.RegisterInstance<ISecuredHttpGateSettings>(securedHttpGateSettings);

            builder.RegisterType<ApplicationLog>().As<IApplicationLog>();
            builder.RegisterType<HttpGate>().As<IHttpGate>();
            builder.RegisterType<SecuredHttpGate>().As<ISecuredHttpGate>();
            builder.RegisterType<TextFileWriter>().As<ITextFileWriter>();
            builder.RegisterType<WebFileSource>().As<IWebFileSource>();
            return builder;
        }

        public static async Task<IServiceCollection> UseVishizhukelAndPeghAsync(this IServiceCollection services, ICsArgumentPrompter csArgumentPrompter) {
            return await UseVishizhukelAndPeghOptionallyDvinAsync(services, csArgumentPrompter, false);
        }

        public static async Task<IServiceCollection> UseVishizhukelDvinAndPeghAsync(this IServiceCollection services, ICsArgumentPrompter csArgumentPrompter) {
            return await UseVishizhukelAndPeghOptionallyDvinAsync(services, csArgumentPrompter, true);
        }

        private static async Task<IServiceCollection> UseVishizhukelAndPeghOptionallyDvinAsync(this IServiceCollection services, ICsArgumentPrompter csArgumentPrompter, bool useDvin) {
            if (useDvin) {
                services.UseDvinAndPegh(csArgumentPrompter);
            } else {
                services.UsePegh(csArgumentPrompter);
            }

            var securedHttpGateSettings = await CreateSecuredHttpGateSettingsAsync(csArgumentPrompter);
            services.AddSingleton<ISecuredHttpGateSettings>(securedHttpGateSettings);

            services.AddTransient<IApplicationLog, ApplicationLog>();
            services.AddTransient<IHttpGate, HttpGate>();
            services.AddTransient<ISecuredHttpGate, SecuredHttpGate>();
            services.AddTransient<ITextFileWriter, TextFileWriter>();
            services.AddTransient<IWebFileSource, WebFileSource>();
            return services;
        }

        private static async Task<SecuredHttpGateSettings> CreateSecuredHttpGateSettingsAsync(ICsArgumentPrompter csArgumentPrompter) {
            var peghContainer = new ContainerBuilder().UsePegh(csArgumentPrompter).Build();
            var repository = peghContainer.Resolve<ISecretRepository>();
            var securedHttpGateSettingsSecret = new SecretSecuredHttpGateSettings();
            var errorsAndInfos = new ErrorsAndInfos();
            var securedHttpGateSettings = await repository.GetAsync(securedHttpGateSettingsSecret, errorsAndInfos);
            if (errorsAndInfos.AnyErrors()) {
                throw new Exception(errorsAndInfos.ErrorsToString());
            }

            return securedHttpGateSettings;
        }
    }
}
