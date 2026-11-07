using Avalonia.Media;
using Avalonia.Styling;

namespace ROGraph.UI.Assets.Themes;

internal record DialogColors
{
    public Color BackgroundColor { get; set; }
    public Color HeaderColor { get; set; }
}

internal record ButtonColors
{
    public Color ConfirmColor { get; set; }
}

internal interface ITheme
{
    ThemeVariant Key { get; init; }
    DialogColors DialogColors { get; set; }
    ButtonColors ButtonColors { get; set; }
}

internal class DefaultTheme : ITheme
{
    public const string DefaultThemeKey = "DefaultThemeKey";
    
    public ThemeVariant Key { get; init; } = new(DefaultThemeKey,  ThemeVariant.Dark);
    
    public DialogColors DialogColors { get; set; } = new()
    {
        BackgroundColor = Color.Parse("#4e4e4e"),
        HeaderColor = Color.Parse("#98915D")
    };
    
    public ButtonColors ButtonColors { get; set; } = new()
    {
        ConfirmColor = Color.Parse("#98915D")
    };
}