import { Project } from './project.model';
import { User } from './user.model';
import { TimeTracking } from './timeTracking.model';
import { Dependency } from './dependency.model';
import { Comment } from './comment.model';
import { File } from './file.model';
import { TaskPriority } from './taskPriority.model'; // Enum tipi
import { TaskStatus } from './taskStatus.model'; // Enum tipi

export interface Task {
  id: number;
  title: string;
  description: string;
  createdAt: Date;
  updatedAt?: Date;
  startDate?: Date;
  dueDate?: Date;
  completedDate?: Date;
  priority: TaskPriority;
  status: TaskStatus;
  projectId: number;
  assignedUserId?: number;
  assignedUser?: User;
}
