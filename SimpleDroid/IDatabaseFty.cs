using SQLite;

namespace SimpleDroid
{
    public interface IDatabaseFty
    {
        SQLiteConnection CreateConnection();
        SQLiteAsyncConnection CreateAsyncConnection();
    }
}