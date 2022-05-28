using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Dvin.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Core;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel {
    public static class VishizhukelContainerBuilder {
        public static async Task<ContainerBuilder> UseVishizhukelAndPeghAsync(this ContainerBuilder builder, string applicationName, ICsArgumentPrompter csArgumentPrompter) {
            return await UseVishizhukelAndPeghOptionallyDvinAsync(builder, applicationName, csArgumentPrompter, false);
        }

        public static async Task<ContainerBuilder> UseVishizhukelDvinAndPeghAsync(this ContainerBuilder builder, string applicationName, ICsArgumentPrompter csArgumentPrompter) {
            return await UseVishizhukelAndPeghOptionallyDvinAsync(builder, applicationName, csArgumentPrompter, true);
        }

        private static async Task<ContainerBuilder> UseVishizhukelAndPeghOptionallyDvinAsync(ContainerBuilder builder, string applicationName, ICsArgumentPrompter csArgumentPrompter, bool useDvin) {
            if (useDvin) {
                builder.UseDvinAndPegh(applicationName, csArgumentPrompter);
            } else {
                builder.UsePegh(applicationName, csArgumentPrompter);
            }

            var securedHttpGateSettings = await CreateSecuredHttpGateSettingsAsync(applicationName, csArgumentPrompter);
            builder.RegisterInstance<ISecuredHttpGateSettings>(securedHttpGateSettings);

            builder.RegisterType<HttpGate>().As<IHttpGate>();
            builder.RegisterType<SecuredHttpGate>().As<ISecuredHttpGate>();
            builder.RegisterType<TextFileWriter>().As<ITextFileWriter>();
            builder.RegisterType<WebFileSource>().As<IWebFileSource>();
            return builder;
        }

        public static async Task<IServiceCollection> UseVishizhukelAndPeghAsync(this IServiceCollection services, string applicationName, ICsArgumentPrompter csArgumentPrompter) {
            return await UseVishizhukelAndPeghOptionallyDvinAsync(services, applicationName, csArgumentPrompter, false);
        }

        public static async Task<IServiceCollection> UseVishizhukelDvinAndPeghAsync(this IServiceCollection services, string applicationName, ICsArgumentPrompter csArgumentPrompter) {
            return await UseVishizhukelAndPeghOptionallyDvinAsync(services, applicationName, csArgumentPrompter, true);
        }

        private static async Task<IServiceCollection> UseVishizhukelAndPeghOptionallyDvinAsync(this IServiceCollection services, string applicationName, ICsArgumentPrompter csArgumentPrompter, bool useDvin) {
            if (useDvin) {
                services.UseDvinAndPegh(applicationName, csArgumentPrompter);
            } else {
                services.UsePegh(applicationName, csArgumentPrompter);
            }

            var securedHttpGateSettings = await CreateSecuredHttpGateSettingsAsync(applicationName, csArgumentPrompter);
            services.AddSingleton<ISecuredHttpGateSettings>(securedHttpGateSettings);

            services.AddTransient<IHttpGate, HttpGate>();
            services.AddTransient<ISecuredHttpGate, SecuredHttpGate>();
            services.AddTransient<ITextFileWriter, TextFileWriter>();
            services.AddTransient<IWebFileSource, WebFileSource>();
            return services;
        }

        private static async Task<SecuredHttpGateSettings> CreateSecuredHttpGateSettingsAsync(string applicationName, ICsArgumentPrompter csArgumentPrompter) {
            var peghContainer = new ContainerBuilder().UsePegh(applicationName, csArgumentPrompter).Build();
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
