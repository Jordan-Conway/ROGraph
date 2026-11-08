using System;
using System.Collections.Generic;
using Avalonia.Styling;
using ROGraph.UI.Assets.Themes;

namespace ROGraph.UI.Services.ThemeService;

internal class ThemeService : IThemeManager, IThemeToggler
{
    private readonly Dictionary<string, ITheme> _themes = [];

    // This will be updated to persist the selected theme
    private string SelectedThemeKey { get; set; } = DefaultTheme.DefaultThemeKey;
    
    public ThemeService()
    {
        var defaultTheme = new DefaultTheme();
        RegisterTheme(DefaultTheme.DefaultThemeKey, defaultTheme);
    }
    
    public event EventHandler<string>? ThemeChanged;
    
    /// <inheritdoc/>
    public ITheme GetTheme(string themeKey)
    {
        var registeredTheme =  _themes.TryGetValue(themeKey, out var theme) ? theme : null;

        return registeredTheme ?? _themes[DefaultTheme.DefaultThemeKey];
    }

    public void SetTheme(string themeKey)
    {
        if (SelectedThemeKey == themeKey)
        {
            return;
        }
        
        SelectedThemeKey = themeKey;
        ThemeChanged?.Invoke(this, SelectedThemeKey);
    }

    public ThemeVariant GetSelectedTheme()
    {
        return _themes[SelectedThemeKey].Key;
    }

    public void RegisterTheme(string key, ITheme theme)
    {
        _themes.Add(key, theme);
    }

    public IReadOnlyList<string> GetAllThemeKeys()
    {
        return new List<string>(_themes.Keys);
    }
}