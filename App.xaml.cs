using System.Windows;

namespace SearchApp;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ThemePluginLoader.ApplyTheme(Resources);
    }
}
