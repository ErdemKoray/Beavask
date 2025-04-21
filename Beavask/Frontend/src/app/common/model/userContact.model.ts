import { User } from './user.model';

export interface UserContact {
  id: number;
  phone: string;
  email: string;
  address: string;
  userId: number;
  user: User;
}
