import { Task } from './task.model';

export interface File {
  id: number;
  fileName: string;
  filePath: string;
  createdAt: Date;
  taskId: number;
  task: Task;
}
