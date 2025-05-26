import { teamMember } from "./teamMember.model";
export interface companyTeam {
  id: number;
  title: string;
  createdAt: Date;
  updatedAt: Date | null;
  teamMembers?: teamMember[];
}