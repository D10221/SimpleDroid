using SQLite;

namespace SimpleDroid
{
    public interface IDatabase
    {
        SQLiteConnection Connection();
    }
}