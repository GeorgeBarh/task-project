import { Component } from '@angular/core';
import { NavComponent } from '../nav/nav.component';
import { AuthService } from '../services/auth.service';
import { UserInfoDto } from '../contracts/auth/user-info.dto';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserDto } from '../contracts/auth/user.dto';

@Component({
  selector: 'app-updateme',
  imports: [FormsModule, CommonModule, NavComponent],
  templateUrl: './updateme.component.html',
  styleUrl: './updateme.component.css',
})
export class UpdatemeComponent {
  authService: AuthService;
  user: UserInfoDto | null;
  OldPassword = '';
  newPassword = '';
  newUsername = '';
  newPortrait = '';
  errorTxt: string = '';
  availablePortraits = [
    'Default.png',
    'Asset 1.png',
    'Asset 2.png',
    'Asset 3.png',
    'Asset 4.png',
    'Asset 5.png',
    'Asset 6.png',
  ];
  constructor(authServive: AuthService, private router: Router) {
    this.authService = authServive;
    this.user = this.authService.getMeLocally();
    this.user?.portrait;
    this.newPortrait = this.user?.portrait || 'Default.png';
  }

  updateMe() {
    if (
      this.OldPassword === '' ||
      this.newPassword === '' ||
      this.newUsername === ''
    ) {
      this.errorTxt = 'All fields are required.';
      return;
    }

    const updatedDto: UserDto = {
      username: this.newUsername,
      password: this.newPassword,
      portrait: this.newPortrait,
    };

    this.authService
      .updateMe(updatedDto, this.OldPassword, this.user?.token!)
      .subscribe({
        next: (response) => {
          if (response.success && response.data) {
            this.authService.saveUserInfo(response.data!);
            this.router.navigate(['/']);
          }
          this.errorTxt = response.message;
        },
        error: (error) => {
          this.errorTxt =
            error.error.message ||
            'An error occurred while updating your profile.';
        },
      });
  }

  deleteMe() {
    this.authService.deleteMe(this.user?.token!).subscribe({
      next: (response) => {
        if (response.success) {
          this.authService.clear();
          this.router.navigate(['/']);
        } else {
          this.errorTxt = response.message;
        }
      },
      error: (error) => {
        this.errorTxt =
          error.error.message ||
          'An error occurred while deleting your account.';
      },
    });
  }
}
