namespace Novelog.Extensions;

internal static class FileExtensions
{
    public static string GetRollingFileName(string filePath, int fileIndex)
    {
        var directory = Path.GetDirectoryName(filePath);
        var filename = Path.GetFileNameWithoutExtension(filePath);
        var extension = Path.GetExtension(filePath);
        
        return directory != null
            ? Path.Combine(directory, $"{filename}_{fileIndex}{extension}")
            : $"{filename}_{fileIndex}{extension}";
    }
}