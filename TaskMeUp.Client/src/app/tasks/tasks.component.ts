import {
  AfterViewInit,
  Component,
  OnInit,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NavComponent } from '../nav/nav.component';
import { GroupService } from '../services/group.service';
import { GroupInfoDto } from '../contracts/group/group-info-dto';
import { PartialTask, TaskStatus } from '../contracts/group/partial-task-dto';
import { CommonModule } from '@angular/common';
import {
  CdkDragDrop,
  CdkDropList,
  DragDropModule,
} from '@angular/cdk/drag-drop';
import { firstValueFrom } from 'rxjs';
import { TaskDetailsComponent } from './task-details/task-details.component';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-tasks',
  imports: [
    NavComponent,
    CommonModule,
    DragDropModule,
    TaskDetailsComponent,
    FormsModule,
  ],
  templateUrl: './tasks.component.html',
  styleUrl: './tasks.component.css',
})
export class TasksComponent implements OnInit, AfterViewInit {
  @ViewChildren(CdkDropList) dropLists!: QueryList<CdkDropList>;
  connectedDropLists: CdkDropList[] = [];
  guid: string = '';
  dropListIds: string[] = [];
  group: GroupInfoDto | null = null;
  showAddTaskDialog = false;
  taskToProcess: PartialTask | null = null;
  newUsername: string = '';

  constructor(
    private route: ActivatedRoute,
    private groupService: GroupService
  ) {}

  ngAfterViewInit(): void {
    // Set connected drop lists after view init
    this.connectedDropLists = this.dropLists.toArray();
  }

  ngOnInit(): void {
    this.guid = this.route.snapshot.paramMap.get('guid') || '';
    console.log('Group GUID:', this.guid);
    try {
      this.dropListIds = Object.values(TaskStatus)
        .filter((value) => typeof value === 'number')
        .map((status) => 'dropList-' + status);
      this.groupService.getGroup(this.guid).subscribe((result) => {
        if (result && result.success && result.data) {
          this.group = result.data; // Example of adding a task to the group
        } else {
          console.error(
            'Failed to fetch group data:',
            result?.message || 'Unknown error'
          );
        }
      });
    } catch (error) {
      console.error('Request error:', error);
    }
  }

  statusNames = [
    { enumValue: TaskStatus.InThoughts, name: 'In Thoughts', intValue: 0 },
    { enumValue: TaskStatus.ToDo, name: 'To Do', intValue: 1 },
    { enumValue: TaskStatus.InProgress, name: 'In Progress', intValue: 2 },
    { enumValue: TaskStatus.Blocked, name: 'Blocked', intValue: 3 },
    { enumValue: TaskStatus.Done, name: 'Done', intValue: 4 },
  ];

  statuses = Object.values(TaskStatus).filter(
    (v) => typeof v === 'number'
  ) as number[];

  getTasksByStatus(status: number) {
    return this.group?.tasks?.filter((task) => task.status === status) || [];
  }
  async onTaskDrop(
    event: CdkDragDrop<any[]>,
    newStatus: number
  ): Promise<void> {
    const task = event.item.data;

    if (task.status !== newStatus) {
      console.log(
        'Task dropped:',
        task,
        'New status:',
        newStatus,
        'old status:',
        task.status
      );

      try {
        const updatedTask: PartialTask = {
          status: newStatus,
          title: task.title,
          description: task.description,
          dueDate: task.dueDate,
          id: task.id,
          assignedUser: task.assignedUser
            ? {
                username: task.assignedUser.username,
                portrait: task.assignedUser.portrait,
              }
            : undefined,
        };

        const result = await firstValueFrom(
          this.groupService.updateTask(this.guid, task.id, updatedTask)
        );

        if (!result || !result.success || !result.data) {
          console.error(
            'Failed to update task:',
            result?.message || 'Unknown error'
          );
          return;
        }

        // Update local task status (since you didn't refresh the entire group)
        task.status = newStatus;

        console.log('Task successfully updated on server.');
      } catch (error) {
        console.error('Request error:', error);
      }
    }
  }
  async onClose() {
    this.showAddTaskDialog = false;
    this.taskToProcess = null;
    try {
      const result = await firstValueFrom(
        this.groupService.getGroup(this.guid)
      );
      if (result && result.success && result.data) {
        this.group = result.data;
      } else {
        console.error(
          'Failed to fetch group data after adding task:',
          result?.message || 'Unknown error'
        );
      }
    } catch (error) {
      console.error('Request error:', error);
    }
  }

  get availableUsers() {
    return (
      this.group?.users?.map((user) => ({
        username: user.username,
        portrait: user.portrait,
      })) || []
    );
  }

  removeUserFromGroup(username: string | undefined) {
    if (!username || !this.guid) return;
    this.groupService.removeUserFromGroup(this.guid, username).subscribe({
      next: () => {
        // Refresh the group data after removal
        this.onClose();
      },
      error: (err) => {
        console.error('Failed to remove user:', err);
      },
    });
  }

  addUserToGroup(username: string | undefined) {
    if (!username || !this.guid) return;
    if (!username.trim()) return;

    this.groupService.addUserToGroup(this.guid, username).subscribe({
      next: () => {
        this.newUsername = '';
        this.onClose();
      },
      error: (err) => {
        console.error('Failed to add user:', err);
      },
    });
  }
}
