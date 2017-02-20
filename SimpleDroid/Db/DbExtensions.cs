using System;
using System.IO;
using SQLite;

namespace SimpleDroid
{
    public static class DbExtensions
    {
        public static void DeleteDatabase(this SQLiteConnection connection)
        {            
            try
            {
                connection.Close();
            }
            catch
            {
                // ...
            }

            if (File.Exists(connection.DatabasePath))
            {

                File.Delete(connection.DatabasePath);
            }            
        }

        public static void CloseConnection(this SQLiteConnection connection)
        {
            connection.Close();
            connection.Dispose();

            // Must be called as the disposal of the connection is not released until the GC runs.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}