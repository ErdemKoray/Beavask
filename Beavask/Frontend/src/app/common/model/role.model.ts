import { RolePermission } from "./rolePermission.model";


export interface Role {
  id: number;
  title: string;
  permissions: RolePermission[];
}
