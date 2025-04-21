import { User } from './user.model';
import { Project } from './project.model';

export interface ProjectMember {
  userId: number;
  user: User;
  projectId: number;
  project: Project;
  role: string;
}
