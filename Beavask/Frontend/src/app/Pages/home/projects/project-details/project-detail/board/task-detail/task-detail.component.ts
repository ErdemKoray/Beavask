import { AfterViewInit, Component, EventEmitter, Input, NgModule, OnInit, Output } from '@angular/core';
import { Task } from '../../../../../../../common/services/task/taskModel/task.model';
import { CommonModule } from '@angular/common';
import { FormsModule, NgModel } from '@angular/forms';
import { TaskService } from '../../../../../../../common/services/task/task.service';
import { AuthprofileService } from '../../../../../../../common/services/profile/authprofile.service';
import { ToastService } from '../../../../../../../components/toast/toast.service';
import { UpdateTaskModel } from '../../../../../../../common/services/task/taskModel/updateTask.model';


@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './task-detail.component.html',
  styleUrl: './task-detail.component.css'
})
export class TaskDetailComponent implements OnInit {
@Output() taskUpdated = new EventEmitter<Task>();
@Output() taskDeleted = new EventEmitter<number>(); 

   @Input() taskD: Task | null = null;

  isEditingDescription = false;
  updatedDescription: string = '';
  reporterName:string='';
  assignedUserName:string='';
  isAssigned:boolean=false;
  statusList = [
  { label: 'Not Started', value: 0 },
  { label: 'In Progress', value: 1 },
  { label: 'Blocked', value: 2 },
  { label: 'On Hold', value: 3 },
  { label: 'Cancelled', value: 4 },
  { label: 'Completed', value: 5 },
];

  constructor(private taskapi:TaskService,private authapi:AuthprofileService,private toastServices:ToastService){

  }

  ngOnInit() {
    console.log('giriyor')
    if (this.taskD) {
      this.updatedDescription = this.taskD.description || '';
      if(this.taskD?.creatorId==null ||this.taskD?.creatorId==0){
        this.reporterName='Undefined'
      }else{
        if(this.taskD.assignedUserId!=0 && this.taskD.assignedUserId!=null){

          this.getAsignee()
        }
        this.getReporter() 
      }
      
    }
  }

  toggleEditDescription(event: MouseEvent) {
    event.stopPropagation();
    this.isEditingDescription = !this.isEditingDescription;
  }



  getReporter(){
    console.log(this.taskD?.creatorId)
    this.taskapi.getReporter(Number(this.taskD?.creatorId)).subscribe({
      next:(response)=>{
      
          this.reporterName=`${response.data.firstName} ${response.data.lastName}`
        
        console.log(response)
      }
    })
  }

  getAsignee(){

    this.taskapi.getReporter(Number(this.taskD?.assignedUserId)).subscribe({
      next:(response)=>{
        
          this.assignedUserName=`${response.data.firstName} ${response.data.lastName}`
          console.log(this.assignedUserName)
        
        console.log(response)
      }
    })
  }



assignTaskToMe() {
  this.authapi.whoami().subscribe({
    next: (response) => {
      const userId = response.userId;
      this.assignedUserName = `${response.firstName} ${response.lastName}`;
      console.log(`${response.firstName} ${response.lastName} ${userId}`);

      this.taskapi.assignTaskToUser(Number(this.taskD?.id), userId).subscribe({
        next: () => {
          this.toastServices.show({
            title: 'Success',
            message: 'Assigned to you successfully.'
          });
          this.isAssigned=true;
        },
        error: (err) => {
          this.toastServices.show({
            title: 'Error',
            message: 'Assignment failed: ' + err?.message
          });
        }
      });
    },
    error: (err) => {
      this.toastServices.show({
        title: 'Error',
        message: 'WhoAmI failed: ' + err?.message
      });
    }
  });
}

saveDescription() {
  if (!this.taskD) return;
  this.authapi.whoami().subscribe({
    next: (response) => {
      const currentUserId = response.userId;
      if (this.taskD!.assignedUserId !== currentUserId) {
        this.toastServices.show({
          title: 'Unauthorized',
          message: 'You are not allowed to update this task.'
        });
        this.isEditingDescription = false;
        return;
      }
      if(this.taskD){
        const updateModel: UpdateTaskModel = {
          title: this.taskD.title,
          description: this.updatedDescription,
          startDate: this.taskD.startDate.toISOString(),
          dueDate: this.taskD.dueDate.toISOString(),
          completedDate: this.taskD.completedDate?.toISOString() ?? new Date().toISOString(),
          updatedAt: new Date().toISOString(),
          priority: this.taskD.priority,
          status: this.taskD.status,
          assignedUserId: this.taskD.assignedUserId ?? 0
        };
  
        this.taskapi.updateTask(this.taskD.id, updateModel).subscribe({
          next: () => {
            this.toastServices.show({
              title: 'Success',
              message: 'Description updated successfully.'
            });
            this.taskD!.description = this.updatedDescription;
            this.taskUpdated.emit(this.taskD!);
            this.isEditingDescription = false;
          },
          error: err => {
            this.toastServices.show({
              title: 'Error',
              message: 'Update failed: ' + (err?.message || 'Unknown error')
            });
          }
        });
      }
    }
  });
}


cancelDescriptionEdit(event:MouseEvent) {
   event.stopPropagation();
  this.updatedDescription = this.taskD?.description || '';
  this.isEditingDescription = false;
}



updateStatus(newStatus: number) {
  if (!this.taskD || this.taskD.status === newStatus) return;

  this.authapi.whoami().subscribe({
    next: (response) => {
      const currentUserId = response.userId;
      if (this.taskD!.assignedUserId !== currentUserId) {
        this.toastServices.show({
          title: 'Unauthorized',
          message: 'Only the assigned user can change the status.'
        });
        return;
      }
    if (this.taskD) {
      const updateModel: UpdateTaskModel = {
        title: this.taskD.title,
        description: this.taskD.description,
        startDate: this.taskD.startDate.toISOString(),
        dueDate: this.taskD.dueDate.toISOString(),
        completedDate: this.taskD.completedDate?.toISOString() ?? new Date().toISOString(),
        updatedAt: new Date().toISOString(),
        priority: this.taskD.priority,
        status: Number(newStatus),
        assignedUserId: this.taskD.assignedUserId ?? 0
      };

      this.taskapi.updateTask(this.taskD.id, updateModel).subscribe({
        next: () => {
          this.taskD!.status = newStatus;
          this.taskUpdated.emit(this.taskD!);
          this.toastServices.show({
            title: 'Success',
            message: 'Status updated.'
          });
        },
        error: err => {
          this.toastServices.show({
            title: 'Error',
            message: 'Status update failed: ' + (err?.error?.message || err.message)
          });
        }
      });
    }
  }
  });
  
}



deleteTask() {
  if (!this.taskD) return;

  this.taskapi.deleteTask(this.taskD.id).subscribe({
    next: () => {
      this.toastServices.show({
        title: 'Success',
        message: 'Task deleted successfully.'
      });
      if(this.taskD)
      this.taskDeleted.emit(this.taskD.id); // board'a bildir
    },
    error: (err) => {
      this.toastServices.show({
        title: 'Error',
        message: 'Deletion failed: ' + err.message
      });
    }
  });
}
}
