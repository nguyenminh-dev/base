using Authen.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Authen;

[DependsOn(
    typeof(AuthenEntityFrameworkCoreTestModule)
    )]
public class AuthenDomainTestModule : AbpModule
{

}
