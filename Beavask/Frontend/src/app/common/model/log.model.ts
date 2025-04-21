import { User } from './user.model';

export interface Log {
  id: number;
  action: string;
  timestamp: Date;
  userId: number;
  user: User;
}
