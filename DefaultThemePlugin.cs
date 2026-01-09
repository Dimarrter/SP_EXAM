using System;
using System.Windows;

namespace SearchApp;

public sealed class DefaultThemePlugin : IThemePlugin
{
    public string Name => "Default Blue";

    public ResourceDictionary CreateResourceDictionary()
    {
        return new ResourceDictionary
        {
            Source = new Uri("ThemeResources.xaml", UriKind.Relative)
        };
    }
}
