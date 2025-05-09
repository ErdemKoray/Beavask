export enum TaskPriority {
  Low = 0,
  Medium = 1,
  High = 2,
  Critical = 3
}

// Mapping function
export function mapPriority(priority: any): TaskPriority {
  switch (priority) {
    case 0:
      return TaskPriority.Low;
    case 1:
      return TaskPriority.Medium;
    case 2:
      return TaskPriority.High;
    case 3:
      return TaskPriority.Critical;
    default:
      return TaskPriority.Low; // Default to Low
  }
}
