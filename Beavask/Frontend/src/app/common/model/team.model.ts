import { Project } from './project.model';
import { User } from './user.model';
export interface Team {
  id: number;
  name: string;
  title?: string; 
  createdAt?: Date;  
  updatedAt?: Date;
}
