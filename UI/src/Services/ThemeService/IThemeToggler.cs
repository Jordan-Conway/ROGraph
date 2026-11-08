using System.Collections.Generic;

namespace ROGraph.UI.Services.ThemeService;

internal interface IThemeToggler
{
    void SetTheme(string themeKey);
    IReadOnlyList<string> GetAllThemeKeys();
}