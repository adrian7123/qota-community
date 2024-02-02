import { Controller, Get, Param, Post } from '@nestjs/common';
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
}
