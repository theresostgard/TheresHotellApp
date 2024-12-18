namespace HotellApp.Services.ServiceFactory
{
    public interface IServiceFactory
    {
        T Get<T>();
    }
}