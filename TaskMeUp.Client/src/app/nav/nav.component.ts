import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { UserInfoDto } from '../contracts/auth/user-info.dto';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  imports: [],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  authService: AuthService;
  user: UserInfoDto | null;
  constructor(authServive: AuthService, private router: Router) {
    this.authService = authServive;
    this.user = this.authService.getMeLocally();
  }

  get imagePath() {
    return 'assets/user-portraits/' + this.user?.portrait || 'Default.png';
  }

  logout() {
    this.authService.clear();
    this.authService.getMeLocally();
  }

  sendMeHome() {
    console.log('Navigating to home');
    this.router.navigate(['/']);
  }

  updateMe() {
    console.log('Navigating to updateme');
    this.router.navigate(['/updateme']);
  }
}
