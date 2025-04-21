import { User } from './user.model';

export interface Event {
  id: number;
  title: string;
  eventDate: Date;
  userId: number;
  user: User;
}
