

  export interface CreateTaskModel {
    title: string;
    description: string;
    startDate: Date;
    dueDate: Date;
    priority: number;
    status: number;
    projectId: number;
    assignedUserId: number;
    }