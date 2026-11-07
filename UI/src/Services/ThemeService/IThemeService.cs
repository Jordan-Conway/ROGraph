using System.Collections.Generic;
using Avalonia.Styling;
using ROGraph.UI.Assets.Themes;

namespace ROGraph.UI.Services.ThemeService;

internal interface IThemeService
{
    /// <summary>
    /// Gets a registered theme for the provided key
    /// </summary>
    /// <param name="themeKey">Key of the theme to get</param>
    /// <returns>The registered key, or the default theme is there is no registered key</returns>
    public ITheme GetTheme(string themeKey);

    public ThemeVariant GetSelectedTheme();

    public void RegisterTheme(string key, ITheme theme);

    public IReadOnlyList<string> GetAllThemeKeys();
}