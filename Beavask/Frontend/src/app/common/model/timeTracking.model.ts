import { Task } from './task.model'; 
import { User } from './user.model'; 

export interface TimeTracking {
  id: number;
  startTime: Date;
  endTime: Date;
  taskId: number;
  task: Task; 
  userId: number;
  user: User; 
}
