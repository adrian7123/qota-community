using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using SocketIOClient;

namespace QotaCommunityPlugin;

[MinimumApiVersion(80)]
public class QotaCommunityPlugin : BasePlugin
{
  public override string ModuleName => "Qota Community";
  public override string ModuleVersion => "0.0.2";
  public override string ModuleAuthor => "https://github.com/adrian7123 (Adrian Bueno)";
  public override string ModuleDescription => "Qota Community Plugin";

  private readonly SocketIO socketClient = new SocketIO("https://f17c-170-79-148-67.ngrok-free.app");

  public override void Load(bool hotReload)
  {
    Logger.LogInformation("Qota Community Plugin loading!");

    Task.Run(socketClient.ConnectAsync);

    // Subscriptions can be added via the instance method
    RegisterEventHandler<EventPlayerDeath>((@event, info) =>
    {

      Logger.LogInformation("Event player death");

      Task.Run(async () =>
      {
        object[] data = {
          @event.Attacker.ClanName,
          @event.Attacker.SteamID,
          @event.Userid.ClanName,
          @event.Userid.SteamID,
        };

        await socketClient.EmitAsync("kill", data);
      });

      // You can use `info.DontBroadcast` to set the dont broadcast flag on the event (in pre handlers)
      // This will prevent the event from being broadcast to other clients.
      // In this example we prevent kill-feed messages from being broadcast if it was not a headshot.
      if (!@event.Headshot)
      {

      }

      return HookResult.Continue;
    }, HookMode.Pre);
  }

  [ConsoleCommand("qc_all_to_spec", "@Qota Community Set All Players to Spec")]
  [RequiresPermissions("@css/admin")]
  public void SetAllToSpecCommand(CCSPlayerController? _, CommandInfo command)
  {
    try
    {
      string[]? commands = command.ArgString.Split(" ");

      Logger.LogInformation($"set_all_to_spec Command Invoked");

      var players = Utilities.GetPlayers();

      foreach (var player in players)
      {
        player.ChangeTeam(CsTeam.Spectator);
      }
    }
    catch (Exception e)
    {
      Logger.LogError(e.Message);
      command.ReplyToCommand(e.Message);
    }
  }

  [ConsoleCommand("qc_change_team", "@Qota Community Change Player Team")]
  [RequiresPermissions("@css/admin")]
  public void ChangeTeamCommand(CCSPlayerController? _, CommandInfo command)
  {
    try
    {
      string[]? commands = command.ArgString.Split(" ");

      int playerID = int.Parse(commands[0]);
      string team = commands[1];

      Logger.LogInformation($"change_team Command Invoked player: {playerID} team: {team}");

      var player = new CCSPlayerController(NativeAPI.GetEntityFromIndex(playerID + 1));

      switch (team)
      {
        case "c":
        case "ct":
          {
            player.ChangeTeam(CsTeam.CounterTerrorist);
            break;
          }
        case "t":
        case "tr":
          {
            player.ChangeTeam(CsTeam.Terrorist);
            break;
          }
        case "s":
        case "spec":
          {
            player.ChangeTeam(CsTeam.Spectator);
            break;
          }
        default:
          {
            command.ReplyToCommand("Parâmetros Incorretos");
            command.ReplyToCommand("Ex: change_team 1 ct");
            break;
          }
      }
    }
    catch (Exception e)
    {
      Logger.LogError(e.Message);
      command.ReplyToCommand(e.Message);
    }
  }
}
