import { Project } from './project.model';
import { User } from './user.model';

export interface Team {
  id: number;
  title: string;
  createdAt: Date;
  projectId: number;
  project: Project;
  members: User[];
}
