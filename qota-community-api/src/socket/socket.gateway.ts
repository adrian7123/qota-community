import {
  WebSocketGateway,
  OnGatewayConnection,
  WebSocketServer,
} from '@nestjs/websockets';
import { Socket } from 'socket.io';
import { SocketService } from './socket.service';

@WebSocketGateway(3033)
export class SocketGateway implements OnGatewayConnection {
  @WebSocketServer()
  private server: Socket;

  constructor(private readonly socketService: SocketService) {}

  handleConnection(socket: Socket): void {
    this.socketService.handleConnection(socket);
  }
}
