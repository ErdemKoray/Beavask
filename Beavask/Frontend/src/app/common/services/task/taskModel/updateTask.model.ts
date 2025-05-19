export interface UpdateTaskModel {
  title: string;
  description: string;
  startDate: string;       // ISO format (string)
  dueDate: string;
  completedDate: string;
  updatedAt: string;
  priority: number;        // 0 = Low, 1 = Medium, ...
  status: number;          // 0 = NotStarted, ...
  assignedUserId: number;
}
