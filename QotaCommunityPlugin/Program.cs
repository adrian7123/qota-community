using SocketIOClient;

internal class Program
{
  private static void Main(string[] args)
  {

    var mainThreadContext = SynchronizationContext.Current ?? new SynchronizationContext();

    Console.WriteLine($"mainThreadContext {mainThreadContext == null}");

    SocketIO client = new SocketIO("http://localhost:3033");

    client.OnConnected += (sender, e) =>
      {
        Console.WriteLine("Socket Connected");
      };

    client.On("execute", (data) =>
    {
      Console.WriteLine("client.On(execute)");
      Console.WriteLine(data.GetValue(0).ToString());

      mainThreadContext?.Post(_ => Console.WriteLine("asd"), null);
    });

    Console.WriteLine("Qota Community Plugin socket try connect start");

    Task.Run(
      client.ConnectAsync
    );

    Console.WriteLine("Qota Community Plugin socket connected");

    Console.ReadLine();
  }
}
