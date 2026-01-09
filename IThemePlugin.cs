using System.Windows;

namespace SearchApp;

public interface IThemePlugin
{
    string Name { get; }

    ResourceDictionary CreateResourceDictionary();
}
