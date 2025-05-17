using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Authen.EntityFrameworkCore;
using Authen.Localization;
using Authen.MultiTenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using TMT.Authentication.Options;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.Timing;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace Authen.Web;

[DependsOn(
    typeof(AuthenHttpApiModule),
    typeof(AuthenApplicationModule),
    typeof(AuthenEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpSettingManagementWebModule),
    typeof(AbpAccountWebIdentityServerModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpTenantManagementWebModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
    )]
public class AuthenWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(AuthenResource),
                typeof(AuthenDomainModule).Assembly,
                typeof(AuthenDomainSharedModule).Assembly,
                typeof(AuthenApplicationModule).Assembly,
                typeof(AuthenApplicationContractsModule).Assembly,
                typeof(AuthenWebModule).Assembly
            );
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureUrls(configuration);
        ConfigureBundles();
        ConfigureAuthentication(context, configuration);
        ConfigureAutoMapper();
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureLocalizationServices();
        ConfigureAutoApiControllers();
        ConfigureSwaggerServices(context, configuration);
        Configure<AbpClockOptions>(option =>
        {
            option.Kind = DateTimeKind.Utc;
        });
        ConfigureAuthenticationModule(configuration, context);
    }

    private void ConfigureAuthenticationModule(IConfiguration configuration, ServiceConfigurationContext context)
    {
        Configure<AuthenOption>(options =>
        {
            configuration.GetSection("AuthenOption").Bind(options);
            options.AccountUrl = configuration["AuthServer:Authority"];
        });

        Configure<AuthServerOption>(options =>
        {
            configuration.GetSection("AuthServer").Bind(options);
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                BasicThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = "Authen";
            });
    }

    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AuthenWebModule>();
        });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<AuthenDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Authen.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<AuthenDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Authen.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<AuthenApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Authen.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<AuthenApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Authen.Application"));
                options.FileSets.ReplaceEmbeddedByPhysical<AuthenWebModule>(hostingEnvironment.ContentRootPath);
            });
        }
    }

    private void ConfigureLocalizationServices()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
            options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
            options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
            options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish"));
            options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
            options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
            options.Languages.Add(new LanguageInfo("is", "is", "Icelandic", "is"));
            options.Languages.Add(new LanguageInfo("it", "it", "Italiano", "it"));
            options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
            options.Languages.Add(new LanguageInfo("ro-RO", "ro-RO", "Română"));
            options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
            options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
            options.Languages.Add(new LanguageInfo("es", "es", "Español"));
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(AuthenApplicationModule).Assembly);
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddOpenApiDocument(document =>
        {
            document.Title = AuthenConsts.ApiDocTitle;
            document.AddSecurity("Bearer", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the bearer scheme",
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Type = OpenApiSecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                Flow = OpenApiOAuth2Flow.Implicit,
            });
            document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
            var descriptions = new Dictionary<string, string>
            {
                { "200", "OK" },
                { "201", "Created" },
                { "202", "Accepted" },
                { "400", "Bad Request" },
                { "401", "Unauthorized" },
                { "403", "Forbidden" },
                { "404", "Not Found" },
                { "405", "Mehtod Not Allowed" },
                { "406", "Not Acceptable" },
                { "500", "Server Error" }
            };

            document.PostProcess = document =>
                            document.Paths.Values
                                .SelectMany(p => p.Values)
                                .SelectMany(p => p.Responses)
                                .Where(r => string.IsNullOrWhiteSpace(r.Value.Description))
                                .ToList()
                                .ForEach(res =>
                                {
                                    if (descriptions.ContainsKey(res.Key))
                                        res.Value.Description = descriptions[res.Key];
                                });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseJwtTokenMiddleware();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.UseOpenApi();
        app.UseSwaggerUi((i) =>
        {
            i.DocumentTitle = AuthenConsts.ApiDocTitle;
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
