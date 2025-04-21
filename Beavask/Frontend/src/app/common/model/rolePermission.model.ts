
import { Permission } from './permission.model';
import { Role } from './role.model';

export interface RolePermission {
  id: number;
  roleId: number;
  role: Role;
  permissionId: number;
  permission: Permission;
}
