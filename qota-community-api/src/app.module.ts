import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { SocketModule } from './socket/socket.module';
import { AdminsModule } from './admins/admins.module';
import { AuthModule } from './auth/auth.module';
import { PlayerModule } from './player/player.module';

@Module({
  imports: [AdminsModule, AuthModule, PlayerModule, SocketModule],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
