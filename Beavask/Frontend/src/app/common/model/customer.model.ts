import { Project } from './project.model';

export interface Customer {
  id: number;
  name: string;
  email: string;
  phone: string;
  projects: Project[];
}
