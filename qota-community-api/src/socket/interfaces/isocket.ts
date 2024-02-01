export interface IPlayerConnected {
  name: string;
  steamID: number;
}
export interface IPlayerKill {
  killedName: string;
  killedSteamID: number;
  killerName: string;
  killerSteamID: number;
  headshot: boolean;
  weapon: string;
}