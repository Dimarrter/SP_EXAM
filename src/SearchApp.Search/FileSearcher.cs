using System.Collections.Generic;
using System.IO;

namespace SearchApp.Search;

public sealed class FileSearcher
{
    public List<SearchResult> Search(string rootPath, string query)
    {
        var results = new List<SearchResult>();
        if (string.IsNullOrWhiteSpace(rootPath) || !Directory.Exists(rootPath))
        {
            return results;
        }

        var normalizedQuery = (query ?? string.Empty).Trim();
        try
        {
            foreach (var filePath in Directory.EnumerateFiles(rootPath, "*", SearchOption.AllDirectories))
            {
                var fileName = Path.GetFileName(filePath);
                if (string.IsNullOrEmpty(normalizedQuery) ||
                    fileName.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(new SearchResult(fileName, filePath));
                }
            }
        }
        catch (IOException)
        {
            // Ignore folders/files we cannot read.
        }
        catch (UnauthorizedAccessException)
        {
            // Ignore folders/files we cannot read.
        }

        return results;
    }
}
