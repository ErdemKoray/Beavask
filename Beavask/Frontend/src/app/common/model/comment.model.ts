import { User } from './user.model';
import { Task } from './task.model';

export interface Comment {
  id: number;
  content: string;
  createdAt: Date;
  userId: number;
  user: User;
  taskId: number;
  task: Task;
}
