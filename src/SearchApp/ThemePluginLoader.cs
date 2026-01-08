using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using SearchApp.Plugins.Abstractions;

namespace SearchApp;

public static class ThemePluginLoader
{
    public static void ApplyTheme(ResourceDictionary resources)
    {
        var plugin = LoadThemePlugin();
        if (plugin is null)
        {
            return;
        }

        var themeResources = plugin.CreateResourceDictionary();
        resources.MergedDictionaries.Add(themeResources);
    }

    private static IThemePlugin? LoadThemePlugin()
    {
        foreach (var plugin in DiscoverPlugins())
        {
            return plugin;
        }

        return null;
    }

    private static IEnumerable<IThemePlugin> DiscoverPlugins()
    {
        var pluginDirectories = new List<string>
        {
            AppContext.BaseDirectory,
            Path.Combine(AppContext.BaseDirectory, "Plugins")
        };

        var assemblyPaths = pluginDirectories
            .Where(Directory.Exists)
            .SelectMany(dir => Directory.EnumerateFiles(dir, "*.dll"))
            .Distinct(StringComparer.OrdinalIgnoreCase);

        foreach (var assemblyPath in assemblyPaths)
        {
            Assembly? assembly;
            try
            {
                assembly = Assembly.LoadFrom(assemblyPath);
            }
            catch (Exception)
            {
                continue;
            }

            foreach (var type in assembly.GetTypes())
            {
                if (!typeof(IThemePlugin).IsAssignableFrom(type) || type.IsAbstract)
                {
                    continue;
                }

                IThemePlugin? plugin;
                try
                {
                    plugin = Activator.CreateInstance(type) as IThemePlugin;
                }
                catch (Exception)
                {
                    continue;
                }

                if (plugin is not null)
                {
                    yield return plugin;
                }
            }
        }
    }
}
