using Microsoft.AspNetCore.SignalR.Client;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:8000/app/ChatHub", (options) => {
                    options.Headers.Add("Apikey", "NomfyDUE47FfECdQT2GK5h51JXLCI5xT");
                })
                .Build();

connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
            });

try
{
    await connection.StartAsync();
    Console.WriteLine("Connection started");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    await connection.InvokeAsync("SendMessage",
        "teste", "teste");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}