using System.Net.Http;
using Microsoft.Extensions.Options;
using TMT.Authentication.Authentications;
using TMT.Authentication.Options;

namespace Authen.WionAccounts;
public class WionAccountSignUpService : SignUpService
{
    public WionAccountSignUpService(IOptions<AuthenOption> authenOption, IHttpClientFactory httpClientFactory, IServerAuthService serverAuthService, IHandlerAfterSignUpService handlerAfterSignUp) : base(authenOption, httpClientFactory, serverAuthService, handlerAfterSignUp)
    {
    }
}
