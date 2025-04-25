import { ProjectMember } from './projectMember.model';
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
  customerId: number;
  customer: Customer;
}
