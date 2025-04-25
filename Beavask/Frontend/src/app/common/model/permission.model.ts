import { RolePermission } from './rolePermission.model';

export interface Permission {
  id: number;
  name: string;
  createdAt: Date;
  updatedAt?: Date;
  roles: RolePermission[];
}
