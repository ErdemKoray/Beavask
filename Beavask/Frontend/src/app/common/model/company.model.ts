import { User } from './user.model'; // User modelini import etmeniz gerekebilir

export interface Company {
  id: number;
  name: string;
  description: string;
  
  // Contact info
  website: string;
  email: string;
  phoneNumber: string;
  logoUrl: string;

  // Address info
  addressLine: string;
  city: string;
  country: string;
  postalCode: string;

  createdAt: Date;
  updatedAt?: Date;
  isActive: boolean;

  // User - Company one-to-many relationship
  users: User[];
}
