export interface PartialTask {
  id: string;
  title: string;
  description?: string;
  dueDate: string; // ISO date string
  status: TaskStatus; // 0-4 as defined in TaskStatus enum
  assignedUser?: {
    username?: string;
    portrait?: string;
  };
}

export enum TaskStatus {
  InThoughts,
  ToDo,
  InProgress,
  Blocked,
  Done,
}
