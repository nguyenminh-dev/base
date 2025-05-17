using Volo.Abp.Modularity;

namespace Authen;

[DependsOn(
    typeof(AuthenApplicationModule),
    typeof(AuthenDomainTestModule)
    )]
public class AuthenApplicationTestModule : AbpModule
{

}
