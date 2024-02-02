import { Controller, Get, Param, Post } from '@nestjs/common';
import axios from 'axios';
import { PrismaService } from 'src/prisma.service';

@Controller('player')
export class PlayerController {
  constructor(private readonly prisma: PrismaService) { }

  @Get("/score/:steamID")
  async GetPlayerScore(@Param() params: any) {
    let score = 0;

    const player = await this.prisma.player.findFirst({
      where: {
        steamID: params.steamID,
      }
    });

    score = player.score;

    return score;
  }

  @Get("/all")
  async GetAllPlayer() {
    let players = await this.prisma.player.findMany({
      orderBy: {
        score: 'desc'
      }
    });
    let playersResult = [];

    const steamIDs = players.map(
      (player) => player.steamID,
    );

    const steam_web_key = process.env.STEAM_WEB_KEY;

    try {
      const res = await axios.get(`http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/`, {
        params: {
          key: steam_web_key,
          steamids: steamIDs.join(","),
        }
      });

      const profiles: any[] = res.data.response.players;

      console.log(profiles);

      players.forEach((player) => {
        const steamProfile = profiles.find((p) => p.steamid === player.steamID);

        playersResult.push({
          ...player,
          ...steamProfile
        });
      });

    } catch (e) {
      console.error(e);
    }

    return playersResult;
  }
}
