import { Company } from "./company.model";
import { Log } from "./log.model";
import { Message } from "../services/message/model/message.model";
import { Problem } from "./problem.model";
import { ProjectMember } from "./projectMember.model";
import { Role } from "./role.model";
import { Team } from "./team.model";

export interface User {
  id: number;
  username: string;
  email: string;
  passwordHash: string;
  passwordSalt: string;
  createdAt: Date;
  updatedAt?: Date;
  isActive: boolean;

  projects: ProjectMember[];
  sentMessages: Message[];
  receivedMessages: Message[];
  userRoles: Role[];
  teamId: number;
  team: Team;
  companyId: number;
  company: Company;
  problems: Problem[];
  logs: Log[];
  notifications: Notification[];
}
