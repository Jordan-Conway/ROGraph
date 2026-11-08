using Avalonia.Media;
using Avalonia.Styling;
using Moq.AutoMock;
using ROGraph.UI.Assets.Themes;
using ROGraph.UI.Services.ThemeService;

namespace ROGraph.UI.Tests.Services;

public sealed class ThemeServiceTests
{
    private readonly AutoMocker _mocker = new();
    private ThemeService _themeService;

    private const string TestThemeKey = "TestThemeKey";
    private readonly TestTheme _testTheme = new();
    
    [SetUp]
    public void Setup()
    {
        _themeService = _mocker.CreateInstance<ThemeService>();
    }

    [Test]
    public void DefaultTheme_IsAlwaysRegistered()
    {
        // Act
        var result = _themeService.GetTheme(DefaultTheme.DefaultThemeKey);
        
        // Assert
        var defaultTheme = new DefaultTheme();
        Assert.That(result, Is.EqualTo(defaultTheme));
    }
    
    [Test]
    public void GetTheme_ThemeRegistered_ReturnsTheme()
    {
        // Arrange
        _themeService.RegisterTheme(TestThemeKey, _testTheme);
        
        // Act
        var result = _themeService.GetTheme(TestThemeKey);
        
        // Assert
        Assert.That(result, Is.EqualTo(_testTheme));
    }

    [Test]
    public void GetTheme_ThemeNotRegistered_ReturnsDefaultTheme()
    {
        // Act
        var result = _themeService.GetTheme("NotRegisteredTheme");
        
        // Assert
        var defaultTheme = new DefaultTheme();
        Assert.That(result, Is.EqualTo(defaultTheme));
    }

    [Test]
    public void SetTheme_ThemeIsDifferent_SetsThemeAndRaisesEvent()
    {
        // Arrange
        var stringFromEvent = string.Empty;
        _themeService.ThemeChanged += Callback;
        _themeService.RegisterTheme(TestThemeKey, _testTheme);
        
        // Act
        _themeService.SetTheme(TestThemeKey);
        
        // Assert
        var currentTheme = _themeService.GetSelectedTheme();
        Assert.Multiple(() =>
        {
            Assert.That(currentTheme, Is.EqualTo(_testTheme.Key));
            Assert.That(stringFromEvent, Is.EqualTo(TestThemeKey));
        });
        return;

        void Callback(object? sender, string newTheme)
        {
            stringFromEvent = newTheme;
        }
    }

    [Test]
    public void SetTheme_ThemeIsSame_EventNotRaised()
    {
        // Arrange
        _themeService.RegisterTheme(TestThemeKey, _testTheme);
        _themeService.SetTheme(TestThemeKey);
        var stringFromEvent = string.Empty;
        _themeService.ThemeChanged += Callback;
        
        // Act
        _themeService.SetTheme(TestThemeKey);
        
        // Assert
        Assert.That(stringFromEvent, Is.EqualTo(string.Empty));
        return;

        void Callback(object? sender, string newTheme)
        {
            stringFromEvent = newTheme;
        }
    }

    [Test]
    public void GetSelectedTheme_ReturnsSelectedTheme()
    {
        // Arrange
        _themeService.RegisterTheme(TestThemeKey, _testTheme);
        _themeService.SetTheme(TestThemeKey);
        
        // Act
        var result = _themeService.GetSelectedTheme();
        
        // Assert
        Assert.That(result, Is.EqualTo(_testTheme.Key));
    }

    [Test]
    public void RegisterTheme_ThemeIsRegistered()
    {
        // Act
        _themeService.RegisterTheme(TestThemeKey, _testTheme);
        
        // Assert
        var theme = _themeService.GetTheme(TestThemeKey);
        Assert.That(theme, Is.EqualTo(_testTheme));
    }

    [Test]
    public void GetAllThemeKeys_ReturnsKeysForAllRegisteredKeys()
    {
        // Arrange
        _themeService.RegisterTheme(TestThemeKey, _testTheme);
        
        // Act
        var result = _themeService.GetAllThemeKeys();
        
        // Assert
        var expected = new string[] { DefaultTheme.DefaultThemeKey, TestThemeKey };
        Assert.That(result, Is.EquivalentTo(expected));
    }
    
    private record TestTheme: DefaultTheme
    {
        public TestTheme()
        {
            Key = new ThemeVariant("TestTheme", ThemeVariant.Light);
            DialogColors.BackgroundColor = Color.Parse("#356412");
        }
    }
}