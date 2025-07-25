import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { UserInfoDto } from '../contracts/auth/user-info.dto';
import { Router } from '@angular/router';
import { NavComponent } from '../nav/nav.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GroupDetailsComponent } from './group-details/group-details.component';
import { PartialGroup } from '../contracts/group/partial-group';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-group',
  imports: [NavComponent, CommonModule, FormsModule, GroupDetailsComponent],
  templateUrl: './group.component.html',
  styleUrl: './group.component.css',
})
export class GroupComponent {
  authService: AuthService;
  user: UserInfoDto | null = null;
  groupNameFilter: string = '';
  tagFilters: string[] = [];
  tagInput: string = '';
  showAddTaskDialog: boolean = false;
  groupToProcess: PartialGroup | null = null;
  filteredGroups;

  constructor(authServive: AuthService, private router: Router) {
    this.authService = authServive;
    const user = this.authService.getMeLocally();
    if (!user) {
      this.router.navigate(['/auth']);
    } else {
      this.user = user;
    }
    this.filteredGroups = this.getfilteredGroups();
  }

  Refresh() {
    const user = this.authService.getMeLocally();
    if (!user) {
      this.router.navigate(['/auth']);
    } else {
      this.user = user;
    }
  }
  getfilteredGroups() {
    return this.user?.groups.filter((group) => {
      const nameMatch = group.name
        .toLowerCase()
        .includes(this.groupNameFilter.toLowerCase());

      const tagsMatch =
        this.tagFilters.length === 0 ||
        this.tagFilters.every((tag) =>
          group.tags.some((t) => t.toLowerCase().includes(tag.toLowerCase()))
        );

      return nameMatch && tagsMatch;
    });
  }

  addTag() {
    const tag = this.tagInput.trim().toLowerCase();
    if (tag && !this.tagFilters.includes(tag)) {
      this.tagFilters.push(tag);
    }
    this.tagInput = '';
    this.filteredGroups = this.getfilteredGroups();
  }

  removeTag(index: number) {
    this.tagFilters.splice(index, 1);
    this.filteredGroups = this.getfilteredGroups();
  }

  addTask() {
    this.showAddTaskDialog = true;
    this.groupToProcess = null;
  }

  async onClose() {
    this.showAddTaskDialog = false;
    this.groupToProcess = null;
    await this.refresh();
  }

  async refresh() {
    try {
      const userInfo = this.authService.getMeLocally();
      if (!userInfo) {
        this.router.navigate(['/auth']);
        return;
      }
      const result = await firstValueFrom(
        this.authService.getMe(userInfo.token)
      );

      if (!result || !result.success || !result.data) {
        console.log('Refresh failed:', result?.message || 'Unknown error');
        return;
      }

      this.user = result.data;
      this.filteredGroups = this.getfilteredGroups();
    } catch (error) {
      console.error('Request error:', error);
    }
  }

  sendToTasks(group: PartialGroup) {
    this.router.navigate(['/group', group.id]);
  }
}
