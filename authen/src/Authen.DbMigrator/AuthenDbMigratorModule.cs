using Authen.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Authen.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AuthenEntityFrameworkCoreModule),
    typeof(AuthenApplicationContractsModule)
    )]
public class AuthenDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
