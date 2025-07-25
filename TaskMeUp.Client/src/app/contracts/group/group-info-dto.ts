import { PartialTask } from './partial-task-dto';

export interface GroupInfoDto {
  id: string;
  name?: string;
  description?: string;
  icon?: string;
  tags?: string[];
  users?: {
    username?: string;
    portrait?: string;
  }[];
  tasks?: PartialTask[];
}
