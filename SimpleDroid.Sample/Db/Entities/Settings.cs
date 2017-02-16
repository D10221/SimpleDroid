using SQLite;

namespace SimpleDroid.Db.Entities
{
    public class Settings : EntityBase
    {
        [PrimaryKey, AutoIncrement]
        public int SettingsId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string ServiceHost
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }


        public int ServicePort
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string UserName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public string Password
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}