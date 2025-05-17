using System;
using System.Collections.Generic;
using System.Text;
using Authen.Localization;
using Volo.Abp.Application.Services;

namespace Authen;

/* Inherit your application services from this class.
 */
public abstract class AuthenAppService : ApplicationService
{
    protected AuthenAppService()
    {
        LocalizationResource = typeof(AuthenResource);
    }
}
