using System.Linq;
using SimpleDroid.Db.Entities;

namespace SimpleDroid.Services
{
    /// <summary>
    /// Auto Registered by TinyIOC
    /// </summary>
    public class SettingService : ISettingService
    {
        private readonly IDatabase _database;

        public SettingService(IDatabase database)
        {
            _database = database;
        }

        public Settings GetFirst()
        {
            using (var connection = _database.Connection())
            {
                return (connection
                            .Query<Settings>("select * from settings where SettingsId > 0")
                            .OrderByDescending(x => x.SettingsId)
                            .FirstOrDefault()
                        ?? new Settings()).SetClean();
            }
        }

        public void Save(Settings settings)
        {
            using (var connection = _database.Connection())
            {
                if (settings.SettingsId == 0)
                {
                    connection.Insert(settings);
                    return;
                }

                connection.Update(settings);
            }
        }
    }
}