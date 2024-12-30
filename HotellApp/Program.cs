using HotellApp.Services;

namespace HotellApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();
            var app = new Application(container);

            app.Run();

        }
    }
}
