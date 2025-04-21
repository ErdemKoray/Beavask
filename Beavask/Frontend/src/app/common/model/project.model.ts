import { ProjectMember } from './project-member.model';
import { Milestone } from './milestone.model';
import { Customer } from './customer.model';
import { Task } from './task.model';

export interface Project {
  id: number;
  name: string;
  description: string;
  createdAt: Date;
  updatedAt?: Date;
  isActive: boolean;
  members: ProjectMember[];
  milestones: Milestone[];
  customerId: number;
  customer: Customer;
  tasks: Task[];
}
