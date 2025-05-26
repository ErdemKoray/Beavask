export interface Project {
  id: number;
  name: string;
  description: string;
  createdAt: Date;
  updatedAt?: Date;
  isActive: boolean;
  customerId: number;
 creatorId?: number;
}
