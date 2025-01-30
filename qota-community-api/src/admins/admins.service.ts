import { Injectable } from '@nestjs/common';
import { Admin } from '@prisma/client';
import { PrismaService } from 'src/prisma.service';
import * as bcrypt from 'bcrypt';

@Injectable()
export class AdminsService {
  constructor(private prisma: PrismaService) {}

  async findOne(email: string): Promise<Admin> {
    return await this.prisma.admin.findUnique({ where: { email } });
  }

  async create(admin: Admin): Promise<Admin> {
    const password = await bcrypt.hash(admin.pass, 10);

    return await this.prisma.admin.create({
      data: { ...admin, pass: password },
    });
  }
}
