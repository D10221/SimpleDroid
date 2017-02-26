namespace SimpleDroid
{
    public static class EnityBaseExtensions
    {
        public static T SetClean<T>(this T entity)
            where T: EntityBase
        {
            entity.SetDirty(false);
            return entity;
        }
    }
}