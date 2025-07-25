import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { PartialUserDto } from '../contracts/auth/partial-user.dto';
import { UserDto } from '../contracts/auth/user.dto';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth',
  imports: [FormsModule, CommonModule],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css',
})
export class AuthComponent {
  errorTxtRegister: string = '';
  errorTxtLogin: string = '';
  loginUsername = '';
  loginPassword = '';
  registerUsername = '';
  registerPassword = '';

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    const loginDto: PartialUserDto = {
      username: this.loginUsername,
      password: this.loginPassword,
    };

    this.authService.login(loginDto).subscribe(
      (result) => {
        if (!result) {
          this.errorTxtLogin =
            'Oops, something went wrong. Please try again later.';
          return;
        }
        if (!result.success || !result.data) {
          this.errorTxtLogin = result.message || 'Login failed';
          return;
        }
        console.log('Login successful:', result);
        if (result.success && result.data) {
          this.errorTxtLogin = '';
        }
        this.authService.saveUserInfo(result.data);
        this.router.navigate(['']);
      },
      (error) => {
        console.log('Request error:', error);
        this.errorTxtLogin =
          'An error occurred during login. Please try again.';
      }
    );
  }

  register() {
    const registerDto: UserDto = {
      username: this.registerUsername,
      password: this.registerPassword,
      portrait: 'Default.png',
    };

    this.authService.register(registerDto).subscribe(
      (result) => {
        if (!result) {
          this.errorTxtLogin =
            'Oops, something went wrong. Please try again later.';
          return;
        }
        if (!result.success || !result.data) {
          this.errorTxtRegister = result.message || 'Login failed';
          return;
        }
        console.log('Login successful:', result);
        if (result.success && result.data) {
          this.errorTxtRegister = '';
        }
        this.authService.saveUserInfo(result.data);
        this.router.navigate(['']);
      },
      (error) => {
        console.log('Request error:', error);
        this.errorTxtLogin =
          'An error occurred during login. Please try again.';
      }
    );
  }
}
