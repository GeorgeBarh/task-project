import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserDto } from '../contracts/auth/user.dto';
import { Observable } from 'rxjs';
import { ApiResult } from '../contracts/api-result';
import { PartialUserDto } from '../contracts/auth/partial-user.dto';
import { UserInfoDto } from '../contracts/auth/user-info.dto';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = 'http://localhost:7008/api/Auth';

  constructor(private http: HttpClient, private router: Router) {}

  register(user: UserDto): Observable<ApiResult<UserInfoDto>> {
    return this.http.post<ApiResult<UserInfoDto>>(
      `${this.baseUrl}/Register`,
      user
    );
  }

  login(user: PartialUserDto): Observable<ApiResult<UserInfoDto>> {
    return this.http.post<ApiResult<UserInfoDto>>(
      `${this.baseUrl}/Login`,
      user
    );
  }

  getMe(token: string): Observable<ApiResult<UserInfoDto>> {
    return this.http.get<ApiResult<UserInfoDto>>(`${this.baseUrl}/getme`, {
      headers: new HttpHeaders().set('Authorization', `Bearer ${token}`),
    });
  }

  updateMe(
    user: UserDto,
    oldPassword: string,
    token: string
  ): Observable<ApiResult<UserInfoDto>> {
    return this.http.put<ApiResult<UserInfoDto>>(
      `${this.baseUrl}/updateme`,
      user,
      {
        headers: new HttpHeaders()
          .set('Authorization', `Bearer ${token}`)
          .set('oldPassword', oldPassword),
      }
    );
  }

  deleteMe(token: string): Observable<ApiResult<UserInfoDto>> {
    return this.http.delete<ApiResult<UserInfoDto>>(
      `${this.baseUrl}/deleteme`,
      {
        headers: new HttpHeaders().set('Authorization', `Bearer ${token}`),
      }
    );
  }

  saveUserInfo(userInfo: UserInfoDto): void {
    localStorage.setItem('userInfo', JSON.stringify(userInfo));
  }

  getUserInfo(): UserInfoDto | null {
    const data = localStorage.getItem('userInfo');
    if (!data) {
      return null;
    }
    return JSON.parse(data) as UserInfoDto;
  }

  clear(): void {
    localStorage.removeItem('userInfo');
  }

  getMeLocally(): UserInfoDto | null {
    const userInfo = this.getUserInfo();
    if (!userInfo) {
      this.router.navigate(['/auth']);
      return null;
    }

    if (!userInfo.token) {
      this.router.navigate(['/auth']);
      return null;
    }
    return userInfo;
  }

  refreshMe(): UserInfoDto | null {
    const userInfo = this.getUserInfo();
    if (!userInfo) {
      this.router.navigate(['/auth']);
      return null;
    }

    this.getMe(userInfo.token).subscribe(
      (result) => {
        if (result.success && result.data) {
          this.saveUserInfo(result.data);
          return result.data;
        } else {
          this.clear();
          this.router.navigate(['/auth']);
          return null;
        }
      },
      (error) => {
        this.clear();
        this.router.navigate(['/auth']);
        return null;
      }
    );
    return this.getUserInfo();
  }
}
