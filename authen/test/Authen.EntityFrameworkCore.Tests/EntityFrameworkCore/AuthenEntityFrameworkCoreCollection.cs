using Xunit;

namespace Authen.EntityFrameworkCore;

[CollectionDefinition(AuthenTestConsts.CollectionDefinitionName)]
public class AuthenEntityFrameworkCoreCollection : ICollectionFixture<AuthenEntityFrameworkCoreFixture>
{

}
