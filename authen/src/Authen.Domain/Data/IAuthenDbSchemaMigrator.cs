using System.Threading.Tasks;

namespace Authen.Data;

public interface IAuthenDbSchemaMigrator
{
    Task MigrateAsync();
}
