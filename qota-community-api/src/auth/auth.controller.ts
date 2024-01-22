import {
  Body,
  Controller,
  Post,
  HttpCode,
  HttpStatus,
  BadRequestException,
} from '@nestjs/common';
import { AuthService } from './auth.service';

@Controller('auth')
export class AuthController {
  constructor(private authService: AuthService) {}

  @HttpCode(HttpStatus.OK)
  @Post('login')
  signIn(@Body() signInDto: Record<string, any>) {
    if (!signInDto.email || !signInDto.pass) {
      return new BadRequestException();
    }

    return this.authService.signIn(signInDto.email, signInDto.pass);
  }
}
