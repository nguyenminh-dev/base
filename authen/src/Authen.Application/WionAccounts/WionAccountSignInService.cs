using System.Net.Http;
using Microsoft.Extensions.Options;
using TMT.Authentication.Authentications;
using TMT.Authentication.Options;

namespace Authen.WionAccounts;
public class WionAccountSignInService : SignInService
{
    public WionAccountSignInService(IOptions<AuthenOption> authenOption, IHttpClientFactory httpClientFactory) : base(authenOption, httpClientFactory)
    {

    }
}
