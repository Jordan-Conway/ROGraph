using Avalonia.Controls;
using ROGraph.UI.Assets.Themes;

namespace ROGraph.UI.Services.ThemeService;

internal static class ThemeConvertor
{
    public static ResourceDictionary ConvertTheme(ITheme theme)
    {
        var resourceDictionary = new ResourceDictionary();
        
        resourceDictionary.Add("ButtonConfirmColor", theme.ButtonColors.ConfirmColor);
        
        resourceDictionary.Add("DialogBackgroundColor", theme.DialogColors.BackgroundColor);
        resourceDictionary.Add("DialogHeaderColor", theme.DialogColors.HeaderColor);
        
        return resourceDictionary;
    }
}