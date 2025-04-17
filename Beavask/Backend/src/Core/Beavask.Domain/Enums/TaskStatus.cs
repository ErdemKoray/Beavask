using System;

namespace Beavask.Domain.Enums
{
    public enum TaskStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Blocked = 2,
        OnHold = 3,
        Completed = 4,
        Cancelled = 5
    }
}
