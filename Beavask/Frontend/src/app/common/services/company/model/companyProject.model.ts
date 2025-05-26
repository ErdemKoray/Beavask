export interface CompanyProject {
  id: number;
  name: string;
  description: string;
  isActive: boolean;
  customerId: number;
  createdAt: string;
  updatedAt?: string | null;
}
