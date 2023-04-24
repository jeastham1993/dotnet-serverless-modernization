using System;
using Topshelf;

namespace QueueProcessor.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<IService>(s =>
                {
                    s.BeforeStartingService(sc => sc.RequestAdditionalTime(TimeSpan.FromSeconds(30)));
                    s.BeforeStoppingService(sc => sc.RequestAdditionalTime(TimeSpan.FromSeconds(30)));

                    s.ConstructUsing(() => new Service());

                    s.WhenStarted(service =>
                    {
                        service.Start();
                    });

                    s.WhenStopped(service =>
                    {
                        service.Stop();
                    });

                    s.WhenShutdown(service =>
                    {
                        service.Stop();
                    });
                });

                x.UseNLog();
                x.EnableShutdown();
                x.RunAsLocalSystem();
                x.StartAutomatically();
            });
        }
    }
}
