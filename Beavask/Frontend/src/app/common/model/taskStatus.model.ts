export enum TaskStatus {
  NotStarted = 0,
  InProgress = 1,

  OnHold = 3,
  Cancelled = 4,
  Completed = 5
}

// Mapping function
export function mapStatus(status: any): TaskStatus {
  switch (status) {
    case 0:
      return TaskStatus.NotStarted;
    case 1:
      return TaskStatus.InProgress;
    case 3:
      return TaskStatus.OnHold;
    case 4:
      return TaskStatus.Cancelled;
    case 5:
      return TaskStatus.Completed;
    default:
      return TaskStatus.NotStarted; // Default to NotStarted
  }
}

  export enum TaskStatusk {
    NotStarted = 'NotStarted',
    InProgress = 'InProgress',
    OnHold = 'OnHold',
    Cancelled = 'Cancelled',
    Completed = 'Completed'
  }
  