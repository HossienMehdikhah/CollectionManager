using System.Reflection;

namespace CollectionManager.Core.Utilities;

public static class PathHelper
{
    public static string GetSqliteDatabaseConnectionString
    {
        get
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var DbPath = Path.Join(path, Assembly.GetExecutingAssembly().FullName.Split('.')[0]);
            if (!Directory.Exists(DbPath))
                Directory.CreateDirectory(DbPath);
            DbPath = Path.Join(DbPath, Assembly.GetExecutingAssembly().FullName.Split('.')[0] + ".db");
            return $"Data Source={DbPath};";
        }
    }
}
