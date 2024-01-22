// using CounterStrikeSharp.API.Core;
// using CounterStrikeSharp.API.Core.Attributes;
// using CounterStrikeSharp.API.Core.Attributes.Registration;
// using CounterStrikeSharp.API.Modules.Commands;
// using Microsoft.Extensions.Logging;
// using CounterStrikeSharp.API;
// using System.Net.WebSockets;
// using Nito.AsyncEx;
// using System.Text;

// namespace QotaCommunityPlugin;

// [MinimumApiVersion(110)]
// public class QotaCommunityPlugin : BasePlugin
// {
//   // private readonly SocketIO client = new SocketIO("http://localhost:3033");


//   private readonly string URL = "ws://localhost:3033/socket.io/?EIO=4&transport=websocket";
//   public override string ModuleName => "Qota Community Plugin";
//   public override string ModuleVersion => "0.0.1";
//   public override string ModuleAuthor => "CounterStrikeSharp & Socket IO";
//   public override string ModuleDescription => "Qota Community Plugin";

//   public string? eventData;


//   private ClientWebSocket? socket;

//   private readonly AsyncLock _mutex = new AsyncLock();

//   private SynchronizationContext? mainThreadContext;

//   public override void Load(bool hotReload)
//   {
//     SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
//   }

//   public async Task EmitAsync(string eventName, string data)
//   {
//     // 42 é um código para identificar mensagens do Socket.IO
//     string message = $"42[\"{eventName}\",\"{data}\"]";
//     await SendAsync(message);
//   }

//   private async Task SendAsync(string message)
//   {
//     try
//     {
//       byte[] buffer = Encoding.UTF8.GetBytes(message);
//       await socket!.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
//     }
//     catch (Exception e)
//     {
//       Logger.LogError(e.Message);
//     }
//   }

//   public void ExecuteCommand(string command)
//   {
//     Console.WriteLine($"ExecuteCommand {command}");
//     Server.ExecuteCommand(command);
//   }

//   [ConsoleCommand("socket", "This is an example command description")]
//   public async void SocketCommand(CCSPlayerController? player, CommandInfo command)
//   {
//     try
//     {
//       using (await _mutex.LockAsync())
//       {
//         ExecuteCommand("kick");
//       }
//     }
//     catch (Exception e)
//     {
//       Logger.LogError(e.Message);
//     }
//   }

//   [ConsoleCommand("conn", "This is an example command description")]
//   public void ConnectCommand(CCSPlayerController? player, CommandInfo command)
//   {
//     mainThreadContext = SynchronizationContext.Current;

//     Console.WriteLine($"mainThreadContext is null {SynchronizationContext.Current == null}");

//     try
//     {
//       socket = new ClientWebSocket();

//       using (_mutex.Lock())
//       {
//         Task.Run(async () =>
//         {
//           await socket.ConnectAsync(new Uri(URL), CancellationToken.None);

//           // Handshake inicial
//           await SendAsync("40");  // 40 significa "abertura de websocket"

//           while (socket.State == WebSocketState.Open)
//           {
//             var buffer = new byte[1024 * 4];
//             var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

//             if (result.MessageType == WebSocketMessageType.Text && result.EndOfMessage)
//             {
//               string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

//               // Aqui você precisa implementar a lógica para lidar com mensagens do Socket.IO
//               // Este é um exemplo simples, pode não cobrir todos os casos do protocolo Socket.IO
//               if (message.StartsWith("42[\"" + "execute" + "\""))
//               {
//                 var parts = message.Split(new[] { "[\"", "\",\"", "\"]" }, StringSplitOptions.None);
//                 if (parts.Length >= 3)
//                 {
//                   eventData = parts[2];
//                 }
//               }
//             }
//           }

//         });

//         while (true)
//         {
//           if (eventData != null)
//           {
//             Console.WriteLine(eventData);
//             ExecuteCommand(eventData);
//             eventData = null;
//           }
//         }
//       }
//     }
//     catch (Exception e)
//     {
//       Logger.LogError(e.Message);
//     }
//   }

//   [ConsoleCommand("css_kick", "This is an example command description")]
//   public void KickCommand(CCSPlayerController? player, CommandInfo command)
//   {
//     ExecuteCommand("kick");
//   }

//   public override void Unload(bool hotReload)
//   {
//     Logger.LogInformation("Qota Community Plugin unloading!");

//     socket?.Dispose();
//   }
// }
