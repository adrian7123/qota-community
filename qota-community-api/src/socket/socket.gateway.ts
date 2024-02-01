import {
  WebSocketGateway,
  OnGatewayConnection,
  WebSocketServer,
  SubscribeMessage,
  MessageBody,
  ConnectedSocket,
} from '@nestjs/websockets';
import { Socket } from 'socket.io';
import { SocketService } from './socket.service';
import { IPlayerConnected, IPlayerKill } from './interfaces/isocket';
import { PrismaService } from 'src/prisma.service';
import { constants } from 'src/shared/constants';

@WebSocketGateway(3033)
export class SocketGateway implements OnGatewayConnection {
  @WebSocketServer()
  private server: Socket;

  constructor(
    private readonly socketService: SocketService,
    private readonly prisma: PrismaService
  ) { }

  handleConnection(socket: Socket): void {
    this.socketService.handleConnection(socket);
  }

  @SubscribeMessage('player connected')
  async handleMessage(@MessageBody() data: IPlayerConnected, @ConnectedSocket() client: Socket) {
    console.info(`Player connected`);
    console.log(data);

    const player = await this.prisma.player.upsert({
      where: {
        steamID: data.steamID.toString(),
      },
      create: {
        name: data.name,
        steamID: data.steamID.toString(),
      },
      update: {
        name: data.name,
        steamID: data.steamID.toString(),
      }
    });

    console.log(player);

    this.server.emit("player connected", player);
  }

  @SubscribeMessage('player kill')
  async handleKill(@MessageBody() data: IPlayerKill, @ConnectedSocket() client: Socket) {
    console.info(`Player ${data.killerName} killed ${data.killedName}`);

    console.log(data);

    let score = 0;

    let player = await this.prisma.player.upsert(
      {
        where: {
          steamID: data.killerSteamID.toString(),
        },
        create: {
          name: data.killerName,
          steamID: data.killerSteamID.toString(),
        },
        update: {
          name: data.killerName,
          steamID: data.killerSteamID.toString(),
        }
      }
    );

    if (data.weapon.includes("knife")) {
      score = player.score + constants.knife;
    } else if (data.weapon.includes("tazer")) {
      score = player.score + constants.taser;
    } else if (data.weapon.includes("deagle")) {
      score = player.score + constants.deagle;
    } else {
      score = player.score + constants.normal;
    }

    if (data.headshot) {
      score += constants.headshot;
    }

    player = await this.prisma.player.update(
      {
        where: {
          steamID: data.killerSteamID.toString(),
        },
        data: {
          name: data.killerName,
          steamID: data.killerSteamID.toString(),
          score,
        }
      }
    );

    console.log(player);

    this.server.emit("player kill", player);
  }
}
