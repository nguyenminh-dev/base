using Authen.EntityFrameworkCore;
using Xunit;

namespace Authen;

[CollectionDefinition(AuthenTestConsts.CollectionDefinitionName)]
public class AuthenDomainCollection : AuthenEntityFrameworkCoreCollectionFixtureBase
{

}
