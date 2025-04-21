import { User } from './user.model';

export interface Notification {
  id: number;
  message: string;
  createdAt: Date;
  userId: number;
  user: User;
}
