using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;
using SocketIOClient;

namespace QotaCommunityPlugin;

[MinimumApiVersion(80)]
public class QotaCommunityPlugin : BasePlugin
{
  public override string ModuleName => "Qota Community Plugin";
  public override string ModuleVersion => "0.0.1";
  public override string ModuleAuthor => "CounterStrikeSharp & Socket IO";
  public override string ModuleDescription => "Qota Community Plugin";

  public override void Load(bool hotReload)
  {
    Logger.LogInformation("Qota Community Plugin loading2!");
  }

  [ConsoleCommand("socket", "This is an example command description")]
  public async void OnCommand(CCSPlayerController? player, CommandInfo command)

  {
    Logger.LogInformation("Qota Community Plugin socket command init");

    var client = new SocketIO("http://localhost:3000/");

    Console.WriteLine(client.Connected);

    client.OnConnected += async (sender, e) =>
    {
      await client.EmitAsync("teste", "socket.io");
    };

    await client.ConnectAsync();

    Console.WriteLine(client.Connected);

    Logger.LogInformation("Qota Community Plugin socket command end");

  }

  public override void Unload(bool hotReload)
  {
    Logger.LogInformation("Qota Community Plugin unloading!");
  }
}
