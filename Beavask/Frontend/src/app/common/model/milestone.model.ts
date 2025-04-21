import { Project } from './project.model';

export interface Milestone {
  id: number;
  name: string;
  dueDate: Date;
  projectId: number;
  project: Project;
}
