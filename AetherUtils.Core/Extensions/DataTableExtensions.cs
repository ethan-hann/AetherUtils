using System.Data;
using System.Text;
using AetherUtils.Core.Files;

namespace AetherUtils.Core.Extensions;

/// <summary>
/// Provides extension methods for manipulating <see cref="DataTable"/> objects.
/// </summary>
public static class DataTableExtensions
{
    /// <summary>
    /// Create and save a generated CSV file from a <see cref="DataTable"/>.
    /// </summary>
    /// <param name="table">The <see cref="DataTable"/> to create a CSV file from.</param>
    /// <param name="filePath">The path of a new CSV file to create or overwrite.</param>
    /// <param name="delimiter">The delimiter to separate fields with; default is <c>,</c>.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="table"/> was <c>null</c>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="filePath"/> was <c>null</c> or empty.</exception>
    public static void ToCsvFile(this DataTable table, string filePath, char delimiter = ',')
    {
        ArgumentNullException.ThrowIfNull(table, nameof(table));
        ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
        
        filePath = FileHelper.ExpandPath(filePath);
        FileHelper.CreateDirectories(filePath, false);
        
        var columns = table.Columns.Cast<DataColumn>().ToArray();
        var lines = (new[] { string.Join(delimiter, columns.Select(c => c.ColumnName)) })
            .Concat(table.Rows.Cast<DataRow>()
                .Select(row => string.Join(delimiter, columns.Select(c => row[c]))));

        StringBuilder sb = new();
        lines.ToList().ForEach(s => sb = sb.AppendLine(s));
        
        FileHelper.SaveFileAsync(filePath, sb.ToString(), false);
    }

    /// <summary>
    /// Create and save individual CSV files based on a <see cref="List{T}"/> containing <see cref="DataTable"/> objects.
    /// </summary>
    /// <param name="tables">The <see cref="List{T}"/> containing <see cref="DataTable"/> objects.</param>
    /// <param name="folderPath">The folder path that the CSV files should be saved into.</param>
    /// <param name="delimiter">The delimiter to separate fields with; default is <c>,</c>.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="tables"/> was <c>null</c>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="folderPath"/> was <c>null</c> or empty.</exception>
    public static void ToCsvFiles(this List<DataTable> tables, string folderPath, char delimiter = ',')
    {
        ArgumentNullException.ThrowIfNull(tables, nameof(tables));
        ArgumentException.ThrowIfNullOrEmpty(folderPath, nameof(folderPath));

        folderPath = FileHelper.ExpandPath(folderPath);
        FileHelper.CreateDirectories(folderPath);
        
        foreach (var table in tables)
        {
            var fileName = $"{table.TableName}.csv";
            var fullPath = Path.Combine(folderPath, fileName);
            table.ToCsvFile(fullPath, delimiter);
        }
    }
}