import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { GroupService } from '../../services/group.service';
import { PartialTask } from '../../contracts/group/partial-task-dto';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-task-details',
  imports: [FormsModule, CommonModule],
  templateUrl: './task-details.component.html',
  styleUrl: './task-details.component.css',
})
export class TaskDetailsComponent implements OnInit {
  @Input({ required: false }) groupId: string | undefined = undefined;
  @Input({ required: false }) task: PartialTask | null = null;
  @Input({ required: false }) availableUsers:
    | {
        username?: string;
        portrait?: string;
      }[]
    | null = null;
  @Output() close = new EventEmitter<void>();
  groupService: GroupService = inject(GroupService);
  title = '';
  description = '';
  dueDate: string | null = null;
  assignedUser: string | null = null;

  ngOnInit() {
    if (this.task) {
      this.title = this.task.title || '';
      this.description = this.task.description || '';
      this.dueDate = this.task.dueDate
        ? this.task.dueDate.substring(0, 10) // get YYYY-MM-DD
        : null;
      this.assignedUser = this.task.assignedUser?.username || null;
    }
    console.log(
      'Available Users:',
      this.availableUsers ? this.availableUsers : 'No users available'
    );
  }
  onCancel() {
    this.close.emit();
  }

  async onSubmit() {
    if (!this.task) {
      const newTask: PartialTask = {
        id: '5ca1d308-06ad-49b2-8582-184ef9a5d49c',
        title: this.title,
        description: this.description,
        assignedUser: this.assignedUser
          ? {
              username: this.assignedUser,
              portrait: this.getAssignedPortrait(),
            }
          : undefined,
        dueDate: this.dueDate
          ? new Date(this.dueDate).toISOString()
          : new Date().toISOString(),
        status: 0,
      };
      try {
        const result = await firstValueFrom(
          this.groupService.createTask(this.groupId || '', newTask)
        );

        if (!result || !result.success || !result.data) {
          console.log('Create failed:', result?.message || 'Unknown error');
          return;
        }

        console.log('Create successful:', result);
        this.close.emit();
      } catch (error) {
        console.error('Create error:', error);
        this.close.emit();
      }
      this.close.emit();
      return;
    }
    const updatedtask: PartialTask = {
      id: this.task.id || '5ca1d308-06ad-49b2-8582-184ef9a5d49c',
      title: this.title,
      description: this.description,
      assignedUser: this.assignedUser
        ? {
            username: this.assignedUser,
            portrait: this.getAssignedPortrait(),
          }
        : undefined,
      dueDate: this.dueDate
        ? new Date(this.dueDate).toISOString()
        : new Date().toISOString(),
      status: this.task.status || 0,
    };
    try {
      const result = await firstValueFrom(
        this.groupService.updateTask(
          this.groupId || '',
          this.task.id,
          updatedtask
        )
      );

      if (!result || !result.success || !result.data) {
        console.log('update failed:', result?.message || 'Unknown error');
        return;
      }

      console.log('update successful:', result);
      this.close.emit();
    } catch (error) {
      console.error('update error:', error);
      this.close.emit();
    }
    this.close.emit();
    return;
  }

  getTitle() {
    return this.task ? 'Edit Task' : 'Create Task';
  }

  getButtonText() {
    return this.task ? 'Update' : 'Create';
  }

  deleteMe() {
    if (!this.task || !this.task.id || !this.groupId) {
      console.error('No task to delete');
      return;
    }

    this.groupService.removeTask(this.groupId, this.task.id).subscribe({
      next: (result) => {
        if (result.success) {
          console.log('Task deleted successfully');
          this.close.emit();
        } else {
          console.error('Failed to delete task:', result.message);
        }
      },
      error: (err) => {
        console.error('Error deleting task:', err);
      },
    });
  }
  getAssignedPortrait(): string {
    if (!this.assignedUser) return 'Default.png';

    const user = this.availableUsers?.find(
      (u) => u.username === this.assignedUser
    );

    return user?.portrait || 'Default.png';
  }

  onUserChange(username: string) {
    this.assignedUser = username || null;
  }
}
