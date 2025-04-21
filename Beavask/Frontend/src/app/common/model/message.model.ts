import { User } from './user.model';

export interface Message {
  id: number;
  content: string;
  sentAt: Date;
  senderId: number;
  sender: User;
  receiverId: number;
  receiver: User;
}
