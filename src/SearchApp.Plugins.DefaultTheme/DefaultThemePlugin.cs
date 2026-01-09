using System;
using System.Windows;

using SearchApp.Plugins.Abstractions;

namespace SearchApp.Plugins.DefaultTheme;

public sealed class DefaultThemePlugin : IThemePlugin
{
    public string Name => "Default Blue";

    public ResourceDictionary CreateResourceDictionary()
    {
        return new ResourceDictionary
        {
            Source = new Uri(
                "pack://application:,,,/SearchApp.Plugins.DefaultTheme;component/ThemeResources.xaml",
                UriKind.Absolute)
        };
    }
}
