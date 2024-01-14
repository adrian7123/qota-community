import { Injectable } from '@nestjs/common';
import { Socket } from 'socket.io';

@Injectable()
export class SocketService {
  private readonly connectedClients: Map<string, Socket> = new Map();

  handleConnection(socket: Socket): void {
    const clientId = socket.id;
    this.connectedClients.set(clientId, socket);

    console.log(clientId);

    socket.on('disconnect', () => {
      console.log(clientId);

      this.connectedClients.delete(clientId);
    });

    socket.on('teste', (data: string): string => {
      console.log(data);
      return 'ssss';
    });
  }
}
