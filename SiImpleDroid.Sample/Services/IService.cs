namespace SimpleDroid.Services
{
    public interface IService<T>
    {
        T GetFirst();

        void Save(T t);
    }
}