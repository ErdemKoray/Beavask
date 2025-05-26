   
 export interface teamMember {
  id?: number;     
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  createdAt: Date;
  updatedAt: Date;
  isActive: boolean;
  teamId: number;
  companyId: number;
}