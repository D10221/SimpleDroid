namespace TinyIoC
{
    /// <summary>
    ///     TinyIoC bootstrapper. Handles auto registration.
    /// </summary>
    public interface ITinyIocBootstraper
    {
        void ConfigureApplicationContainer(TinyIoCContainer container);
    }
}