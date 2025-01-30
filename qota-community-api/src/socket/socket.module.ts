import { Module } from '@nestjs/common';
import { SocketGateway } from './socket.gateway';
import { SocketService } from './socket.service';
import { PrismaService } from 'src/prisma.service';

@Module({
  providers: [SocketGateway, SocketService, PrismaService],
})
export class SocketModule {}
