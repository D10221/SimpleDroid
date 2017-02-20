using System;
using System.IO;
using SQLite;

namespace SimpleDroid
{
    public class DatabaseFty : IDatabaseFty
    {
        private string _sqliteFilename;
        private string _databasePath;
            
        public string SqliteFilename => _sqliteFilename 
                                        ?? (_sqliteFilename = "SimpleDroid.Sample.db3");

        public string DatabasePath => _databasePath 
                                      ?? (_databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), SqliteFilename));

        public bool StoreDateTimeAsTicks { get; set; } = true;

        public SQLiteConnection CreateConnection()
        {                
            return new SQLiteConnection(DatabasePath, StoreDateTimeAsTicks);
        }

        public SQLiteAsyncConnection CreateAsyncConnection()
        {                                
            return new SQLiteAsyncConnection(DatabasePath,StoreDateTimeAsTicks);
        }           
    }
}