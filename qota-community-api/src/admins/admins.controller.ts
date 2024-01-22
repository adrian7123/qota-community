import { Body, ConflictException, Controller, Post } from '@nestjs/common';
import { AdminsService } from './admins.service';
import { Admin } from '@prisma/client';

@Controller('admin')
export class AdminsController {
  constructor(private readonly adminsService: AdminsService) {}

  @Post('create')
  async createAdmin(@Body() admin: Admin) {
    const admin_exists = await this.adminsService.findOne(admin.email);

    if (admin_exists) {
      return new ConflictException('Email já cadastrado');
    }

    return await this.adminsService.create(admin);
  }
}
