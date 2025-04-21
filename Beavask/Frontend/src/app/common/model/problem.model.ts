import { User } from './user.model';

export interface Problem {
  id: number;
  title: string;
  description: string;
  createdAt: Date;
  updatedAt?: Date;
  isActive: boolean;
  user: User;
  userId: number;
}
