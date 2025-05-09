
import { User } from "../../../model/user.model";
import { ProjectDetail } from "../../projects/ProjectDetail.model";


  export interface Task {
    id: number;
    title: string;
    description: string;
    createdAt: Date;
    updatedAt: Date | null;
    startDate: Date;
    dueDate: Date;
    completedDate: Date | null;
    isActive: boolean;
    priority: number; 
    status: number; 
    projectId: number;
    project: ProjectDetail | null; 
    creatorId: number | null; 
    assignedUserId: number | null; 
    assignedUser: User | null; 
  }