using System.Text.Json;
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

  private readonly SocketIO socketClient = new SocketIO("http://15.229.76.232:3033");

  private readonly HttpClient client = new HttpClient();

  private readonly string qotaPrefix = $"[{ChatColors.Green}Qota Community{ChatColors.Default}] ";

  public override void Load(bool hotReload)
  {
    Logger.LogInformation("Qota Community Plugin loading!");

    Task.Run(async () =>
    {
      await socketClient.ConnectAsync();
      Logger.LogInformation("Qota Community Socket Connected");
    });

  }

  [GameEventHandler(HookMode.Pre)]
  public HookResult OnPlayerConnect(EventPlayerConnect @event, GameEventInfo info)
  {

    Logger.LogInformation($"@Qota Community Player {@event.Userid.SteamID} Connected");

    socketClient.EmitAsync("player connected", new Dictionary<string, dynamic> {
      {"name", @event.Userid.PlayerName},
      {"steamID", $"{@event.Userid.SteamID}"}
    });

    return HookResult.Continue;
  }

  [GameEventHandler(HookMode.Pre)]
  public HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
  {
    Logger.LogInformation($"Event player death {@event.Attacker.PlayerName}");

    socketClient.EmitAsync("player kill", new Dictionary<string, dynamic> {
      {"killedName",@event.Userid.PlayerName},
      {"killedSteamID",$"{@event.Userid.SteamID}"},
      {"killerName",@event.Attacker.PlayerName},
      {"killerSteamID",$"{@event.Attacker.SteamID}"},
      {"headshot",@event.Headshot},
      {"weapon",@event.Weapon},
    });

    double score = 30;

    if (@event.Weapon.Contains("knife"))
    {
      score = 100;
    }
    else if (@event.Weapon.Contains("tazer"))
    {
      score = 80;
    }
    else if (@event.Weapon.Contains("deagle"))
    {
      score = 60;
    }

    if (@event.Headshot)
    {
      score += 20;
    }

    var s = Convert.ToInt32(score / 3);

    var shotguns = new List<string> {
      "mag7",
      "sawedoff",
      "nova",
      "xm1014"
    };

    if (shotguns.Contains(@event.Weapon))
    {
      score = 0.1;
    }

    if (@event.Weapon.Contains("knife"))
    {
      s = 200;
    }

    @event.Attacker.PrintToChat($"{qotaPrefix}Matou {ChatColors.Green}+{score}{ChatColors.Green}");
    @event.Userid.PrintToChat($"{qotaPrefix}Morreu {ChatColors.Red}-{s}{ChatColors.Red}");

    return HookResult.Continue;
  }

  [ConsoleCommand("points", "@Qota Community Get Points")]
  public void GetPoints(CCSPlayerController? player, CommandInfo command)
  {

    Logger.LogInformation("GetPoints");

    if (player == null) return;

    Task.Run(async () =>
    {
      var res = await client.GetAsync($"http://api.rank.buenoo.online/player/score/{player?.SteamID}");

      if (res.IsSuccessStatusCode)
      {
        var score = res.Content.ToString();
        Logger.LogInformation(score);

        player?.PrintToChat($"{qotaPrefix}seu score é {score}");
      }
    });
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
