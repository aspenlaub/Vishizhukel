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

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel;

public static class VishizhukelContainerBuilder {
    public static async Task<ContainerBuilder> UseVishizhukelAndPeghAsync(this ContainerBuilder builder, string applicationName) {
        return await UseVishizhukelAndPeghOptionallyDvinAsync(builder, applicationName, false);
    }

    public static async Task<ContainerBuilder> UseVishizhukelDvinAndPeghAsync(this ContainerBuilder builder, string applicationName) {
        return await UseVishizhukelAndPeghOptionallyDvinAsync(builder, applicationName, true);
    }

    private static async Task<ContainerBuilder> UseVishizhukelAndPeghOptionallyDvinAsync(ContainerBuilder builder, string applicationName, bool useDvin) {
        if (useDvin) {
            builder.UseDvinAndPegh(applicationName);
        } else {
            builder.UsePegh(applicationName);
        }

        SecuredHttpGateSettings securedHttpGateSettings = await CreateSecuredHttpGateSettingsAsync(applicationName);
        builder.RegisterInstance<ISecuredHttpGateSettings>(securedHttpGateSettings);

        builder.RegisterType<HttpGate>().As<IHttpGate>();
        builder.RegisterType<SecuredHttpGate>().As<ISecuredHttpGate>();
        builder.RegisterType<TextFileWriter>().As<ITextFileWriter>();
        builder.RegisterType<WebFileSource>().As<IWebFileSource>();
        return builder;
    }

    public static async Task<IServiceCollection> UseVishizhukelAndPeghAsync(this IServiceCollection services, string applicationName) {
        return await UseVishizhukelAndPeghOptionallyDvinAsync(services, applicationName, false);
    }

    public static async Task<IServiceCollection> UseVishizhukelDvinAndPeghAsync(this IServiceCollection services, string applicationName) {
        return await UseVishizhukelAndPeghOptionallyDvinAsync(services, applicationName, true);
    }

    private static async Task<IServiceCollection> UseVishizhukelAndPeghOptionallyDvinAsync(this IServiceCollection services, string applicationName, bool useDvin) {
        if (useDvin) {
            services.UseDvinAndPegh(applicationName);
        } else {
            services.UsePegh(applicationName);
        }

        SecuredHttpGateSettings securedHttpGateSettings = await CreateSecuredHttpGateSettingsAsync(applicationName);
        services.AddSingleton<ISecuredHttpGateSettings>(securedHttpGateSettings);

        services.AddTransient<IHttpGate, HttpGate>();
        services.AddTransient<ISecuredHttpGate, SecuredHttpGate>();
        services.AddTransient<ITextFileWriter, TextFileWriter>();
        services.AddTransient<IWebFileSource, WebFileSource>();
        return services;
    }

    private static async Task<SecuredHttpGateSettings> CreateSecuredHttpGateSettingsAsync(string applicationName) {
        IContainer peghContainer = new ContainerBuilder().UsePegh(applicationName).Build();
        ISecretRepository repository = peghContainer.Resolve<ISecretRepository>();
        var securedHttpGateSettingsSecret = new SecretSecuredHttpGateSettings();
        var errorsAndInfos = new ErrorsAndInfos();
        SecuredHttpGateSettings securedHttpGateSettings = await repository.GetAsync(securedHttpGateSettingsSecret, errorsAndInfos);
        return errorsAndInfos.AnyErrors()
            ? throw new Exception(errorsAndInfos.ErrorsToString())
            : securedHttpGateSettings;
    }
}