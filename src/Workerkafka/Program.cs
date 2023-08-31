using Common.Models;
using Workerkafka;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSignalR().AddStackExchangeRedis(hostContext.Configuration.GetConnectionString("Redis") ?? "");
        services.AddSingleton<MongoContext>();

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
