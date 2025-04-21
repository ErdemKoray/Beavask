import { Task } from './task.model';

export interface Dependency {
  id: number;
  title: string;
  taskId: number;
  task: Task;
}
