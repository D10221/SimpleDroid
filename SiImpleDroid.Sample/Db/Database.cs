using NLog;
using SQLite;

namespace SimpleDroid.Db
{
    class Database : IDatabase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        readonly IDatabaseFty _databaseFty;

        private static bool _initialized;

        public Database(IDatabaseFty databaseFty)
        {
            _databaseFty = databaseFty;

            if (_initialized) return;
            _initialized = true;

            _databaseFty.CreateConnection().CreateTable<Entities.Settings>();
        }

        public SQLiteConnection Connection()
        {
            return _databaseFty.CreateConnection();
        }
    }
}