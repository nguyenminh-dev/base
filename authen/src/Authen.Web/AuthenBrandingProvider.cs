using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Authen.Web;

[Dependency(ReplaceServices = true)]
public class AuthenBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Authen";
}
