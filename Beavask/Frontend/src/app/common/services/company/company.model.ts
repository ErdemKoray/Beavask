import { User } from "../../model/user.model";

export interface Company {
  id: number;
  name: string;
  description: string;
  website: string;
  email: string;
  phoneNumber: string;
  logoUrl: string;
  addressLine: string;
  city: string;
  country: string;
  postalCode: string;
  createdAt: string;
  updatedAt: string;
  isActive: boolean;
    users?: User[];
}
