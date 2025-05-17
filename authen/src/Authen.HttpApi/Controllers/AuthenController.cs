using Authen.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Authen.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AuthenController : AbpControllerBase
{
    protected AuthenController()
    {
        LocalizationResource = typeof(AuthenResource);
    }
}
