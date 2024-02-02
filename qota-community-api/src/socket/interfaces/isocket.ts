export interface IPlayerConnected {
  name: string;
  steamID: string;
}
export interface IPlayerKill {
  killedName: string;
  killedSteamID: string;
  killerName: string;
  killerSteamID: string;
  headshot: boolean;
  weapon: string;
}