using Authen.EntityFrameworkCore;
using Xunit;

namespace Authen;

[CollectionDefinition(AuthenTestConsts.CollectionDefinitionName)]
public class AuthenWebCollection : AuthenEntityFrameworkCoreCollectionFixtureBase
{

}
