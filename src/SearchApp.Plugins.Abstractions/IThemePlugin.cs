using System.Windows;

namespace SearchApp.Plugins.Abstractions;

public interface IThemePlugin
{
    string Name { get; }

    ResourceDictionary CreateResourceDictionary();
}
