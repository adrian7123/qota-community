import { Injectable, UnauthorizedException } from '@nestjs/common';
import { JwtService } from '@nestjs/jwt';
import { AdminsService } from 'src/admins/admins.service';
import * as bcrypt from 'bcrypt';

@Injectable()
export class AuthService {
  constructor(
    private adminService: AdminsService,
    private jwtService: JwtService,
  ) {}

  async signIn(email: string, password: string): Promise<any> {
    const admin = await this.adminService.findOne(email);

    const passEquals = await bcrypt.compare(password, admin.pass);

    if (!passEquals) {
      throw new UnauthorizedException();
    }

    const payload = { sub: admin.id, email: admin.email };
    return {
      ...admin,
      access_token: await this.jwtService.signAsync(payload, {
        secret: `${process.env.JWT_SECRET}`,
      }),
    };
  }
}
