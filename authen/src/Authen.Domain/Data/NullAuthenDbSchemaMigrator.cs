using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Authen.Data;

/* This is used if database provider does't define
 * IAuthenDbSchemaMigrator implementation.
 */
public class NullAuthenDbSchemaMigrator : IAuthenDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
