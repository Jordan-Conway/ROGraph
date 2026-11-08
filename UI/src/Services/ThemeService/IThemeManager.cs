using System;
using System.Collections.Generic;
using Avalonia.Styling;
using ROGraph.UI.Assets.Themes;

namespace ROGraph.UI.Services.ThemeService;

internal interface IThemeManager
{
    event EventHandler<string>? ThemeChanged;
    
    /// <summary>
    /// Gets a registered theme for the provided key
    /// </summary>
    /// <param name="themeKey">Key of the theme to get</param>
    /// <returns>The registered key, or the default theme is there is no registered key</returns>
    
    ITheme GetTheme(string themeKey);

    void SetTheme(string themeKey);
    
    void RegisterTheme(string key, ITheme theme);

    ThemeVariant GetSelectedTheme();

    IReadOnlyList<string> GetAllThemeKeys();
}