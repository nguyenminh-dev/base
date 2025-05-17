using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Authen.Pages;

[Collection(AuthenTestConsts.CollectionDefinitionName)]
public class Index_Tests : AuthenWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
