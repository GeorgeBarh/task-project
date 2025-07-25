import {
  Component,
  Output,
  EventEmitter,
  inject,
  Input,
  OnInit,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PartialGroup } from '../../contracts/group/partial-group';
import { GroupService } from '../../services/group.service';
import { GroupDto } from '../../contracts/group/group-dto';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-group-details',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './group-details.component.html',
  styleUrl: './group-details.component.css',
})
export class GroupDetailsComponent implements OnInit {
  @Input({ required: false }) group: PartialGroup | null = null;
  @Output() close = new EventEmitter<void>();
  title = '';
  description = '';
  tags = '';
  groupService: GroupService = inject(GroupService);

  ngOnInit() {
    if (this.group) {
      this.title = this.group.name || '';
      this.description = this.group.description || '';
      this.tags = this.group.tags ? this.group.tags.join(', ') : '';
    }
    console.log('GroupDetailsComponent initialized with group:', this.group);
  }

  onCancel() {
    this.close.emit();
  }

  async onSubmit() {
    if (!this.group) {
      const createGroup: GroupDto = {
        name: this.title,
        description: this.description,
        tags: this.tags.split(',').map((tag) => tag.trim()),
      };

      try {
        const result = await firstValueFrom(
          this.groupService.createGroup(createGroup)
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

    const updatedGroup: GroupDto = {
      name: this.title,
      description: this.description,
      tags: this.tags.split(',').map((tag) => tag.trim()),
    };

    try {
      const result = await firstValueFrom(
        this.groupService.updateGroup(this.group.id, updatedGroup)
      );

      if (!result || !result.success || !result.data) {
        console.log('Update failed:', result?.message || 'Unknown error');
        return;
      }

      console.log('Update successful:', result);
      this.close.emit();
    } catch (error) {
      console.error('Request error:', error);
      this.close.emit();
    }
  }

  getTitle() {
    return this.group ? 'Edit Group' : 'Create Group';
  }

  getButtonText() {
    return this.group ? 'Update' : 'Create';
  }
}
