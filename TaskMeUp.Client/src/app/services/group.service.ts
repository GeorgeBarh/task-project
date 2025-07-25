import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GroupDto } from '../contracts/group/group-dto';
import { AuthService } from './auth.service';
import { PartialTask } from '../contracts/group/partial-task-dto';

@Injectable({
  providedIn: 'root',
})
export class GroupService {
  private baseUrl = 'http://localhost:7008/api/Group';

  constructor(private http: HttpClient, private authService: AuthService) {}

  getToken() {
    return this.authService.getMeLocally()!.token || '';
  }
  createGroup(group: GroupDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/CreateGroup`, group, {
      headers: new HttpHeaders().set(
        'Authorization',
        `Bearer ${this.getToken()}`
      ),
    });
  }

  updateGroup(groupId: string, group: GroupDto): Observable<any> {
    const headers = new HttpHeaders()
      .set('groupId', groupId)
      .set('Authorization', `Bearer ${this.getToken()}`);
    return this.http.put(`${this.baseUrl}/UpdateGroup`, group, { headers });
  }

  deleteGroup(groupId: string): Observable<any> {
    const headers = new HttpHeaders()
      .set('groupId', groupId)
      .set('Authorization', `Bearer ${this.getToken()}`);
    return this.http.delete(`${this.baseUrl}/DeleteGroup`, { headers });
  }

  getGroup(groupId: string): Observable<any> {
    const headers = new HttpHeaders()
      .set('groupId', groupId)
      .set('Authorization', `Bearer ${this.getToken()}`);
    return this.http.get(`${this.baseUrl}/GetGroup`, { headers });
  }

  addUserToGroup(groupId: string, username: string): Observable<any> {
    const headers = new HttpHeaders()
      .set('groupId', groupId)
      .set('username', username)
      .set('Authorization', `Bearer ${this.getToken()}`);
    return this.http.put(`${this.baseUrl}/AddUserToGroup`, {}, { headers });
  }

  removeUserFromGroup(groupId: string, username: string): Observable<any> {
    const headers = new HttpHeaders()
      .set('groupId', groupId)
      .set('username', username)
      .set('Authorization', `Bearer ${this.getToken()}`);
    return this.http.put(
      `${this.baseUrl}/RemoveUserFromGroup`,
      {},
      { headers }
    );
  }
  createTask(groupId: string, task: PartialTask): Observable<any> {
    const headers = new HttpHeaders()
      .set('groupId', groupId)
      .set('Authorization', `Bearer ${this.getToken()}`);
    return this.http.post(`${this.baseUrl}/CreateTask`, task, { headers });
  }

  updateTask(
    groupId: string,
    taskId: string,
    task: PartialTask
  ): Observable<any> {
    const headers = new HttpHeaders()
      .set('groupId', groupId)
      .set('taskId', taskId)
      .set('Authorization', `Bearer ${this.getToken()}`);
    return this.http.put(`${this.baseUrl}/UpdateTask`, task, { headers });
  }

  removeTask(groupId: string, taskId: string): Observable<any> {
    const headers = new HttpHeaders()
      .set('groupId', groupId)
      .set('taskId', taskId)
      .set('Authorization', `Bearer ${this.getToken()}`);
    return this.http.delete(`${this.baseUrl}/RemoveTask`, { headers });
  }
}
