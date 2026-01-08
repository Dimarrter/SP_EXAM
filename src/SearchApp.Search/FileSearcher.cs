using System.Collections.Generic;
using System.IO;

namespace SearchApp.Search;

public sealed class FileSearcher
{
    public IEnumerable<SearchResult> Search(string rootPath, string query)
    {
        if (string.IsNullOrWhiteSpace(rootPath) || !Directory.Exists(rootPath))
        {
            yield break;
        }

        query = query?.Trim() ?? string.Empty;
        foreach (var filePath in EnumerateFilesSafe(rootPath))
        {
            var fileName = Path.GetFileName(filePath);
            if (string.IsNullOrEmpty(query) || fileName.Contains(query, StringComparison.OrdinalIgnoreCase))
            {
                yield return new SearchResult(fileName, filePath);
            }
        }
    }

    private static IEnumerable<string> EnumerateFilesSafe(string rootPath)
    {
        var directories = new Stack<string>();
        directories.Push(rootPath);

        while (directories.Count > 0)
        {
            var current = directories.Pop();
            IEnumerable<string> files;
            try
            {
                files = Directory.EnumerateFiles(current);
            }
            catch (IOException)
            {
                continue;
            }
            catch (UnauthorizedAccessException)
            {
                continue;
            }

            foreach (var file in files)
            {
                yield return file;
            }

            IEnumerable<string> subDirectories;
            try
            {
                subDirectories = Directory.EnumerateDirectories(current);
            }
            catch (IOException)
            {
                continue;
            }
            catch (UnauthorizedAccessException)
            {
                continue;
            }

            foreach (var directory in subDirectories)
            {
                directories.Push(directory);
            }
        }
    }
}
