using Volo.Abp.Settings;

namespace Authen.Settings;

public class AuthenSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AuthenSettings.MySetting1));
    }
}
