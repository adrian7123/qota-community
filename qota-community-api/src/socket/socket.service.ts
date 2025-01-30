import { Injectable } from '@nestjs/common';
import { Socket } from 'socket.io';

@Injectable()
export class SocketService {
  private readonly connectedClients: Map<string, Socket> = new Map();

  handleConnection(socket: Socket): void {
    const clientId = socket.id;
    this.connectedClients.set(clientId, socket);

    console.log('Connected ', clientId);

    socket.on('disconnect', () => {
      console.log('Disconnected ', clientId);

      this.connectedClients.delete(clientId);
    });
  }
}
