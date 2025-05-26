import { AfterViewInit, Component, EventEmitter, Input, NgModule, OnInit, Output } from '@angular/core';
import { Task } from '../../../../../../../common/services/task/taskModel/task.model';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, NgModel } from '@angular/forms';
import { TaskService } from '../../../../../../../common/services/task/task.service';
import { AuthprofileService } from '../../../../../../../common/services/profile/authprofile.service';
import { ToastService } from '../../../../../../../components/toast/toast.service';
import { UpdateTaskModel } from '../../../../../../../common/services/task/taskModel/updateTask.model';
import { CommentService } from '../../../../../../../common/services/comment/comment.service';
import { CreateComment } from '../../../../../../../common/model/comment/createcomment.model';
import { Comment } from '../../../../../../../common/model/comment/comment.model';
import { CompanyService } from '../../../../../../../common/services/company/company.service';
import { AuthCompanyService } from '../../../../../../../common/services/company/auth-company.service';



@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [CommonModule,FormsModule,DatePipe],
  templateUrl: './task-detail.component.html',
  styleUrl: './task-detail.component.css'
})
export class TaskDetailComponent implements OnInit {
@Output() taskUpdated = new EventEmitter<Task>();
@Output() taskDeleted = new EventEmitter<number>();  
@Output() taskDeactive= new EventEmitter<boolean>(); 

@Input() taskD: Task | null = null;
 assignees: { userId: number; fullName: string }[] = [];
  assignedUserId: number | null = null;
selectedAssigneeId?: number;

  comments: Comment[] = [];
  newCommentTitle: string = '';
  newCommentContent: string = '';
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

  constructor(private taskapi:TaskService,
    private authapi:AuthprofileService,
    private toastServices:ToastService,
    private commentService: CommentService,
    private companyService:CompanyService,
    private authCompanyService:AuthCompanyService
  ){

  }

  ngOnInit() {
    if (this.taskD) {
          this.loadAssignees();

      this.updatedDescription = this.taskD.description || '';
      if(this.taskD?.creatorId==null ||this.taskD?.creatorId==0){
        this.reporterName='Undefined'
      }else{
        if(this.taskD.assignedUserId!=0 && this.taskD.assignedUserId!=null){

          this.getAsignee()
           this.assignedUserId = this.taskD.assignedUserId ?? null;
        }
        this.getReporter() 
      }
        this.loadComments(this.taskD.id);
    }
  }

  toggleEditDescription(event: MouseEvent) {
    event.stopPropagation();
    this.isEditingDescription = !this.isEditingDescription;
  }

loadAssignees(): void {
  if (!this.taskD) return;
  this.authCompanyService.getWhoamiCompany().subscribe({
    next: (company) => {
      console.log(company);
      this.reporterName=company.companyName;
      const companyId = company.companyId;
      if (!companyId) {
        this.toastServices.show({ title: 'Error', message: 'Company information not found.' });
        return;
      }
      console.log("loadassigne");

      this.companyService.getCompanyUsers(Number(this.taskD?.projectId)).subscribe({
        next: (res) => {
          console.log("loadassigne");
          console.log(res);
          const users = res ?? []; // eğer ApiResponse varsa, res.data olarak düzeltin
          this.assignees = users.map(u => ({
            userId: u.id,
            fullName: `${ u.username}`.trim()
          }));
          if (this.taskD?.assignedUserId)
            this.selectedAssigneeId = this.taskD.assignedUserId;
        },
        error: () => {
          this.toastServices.show({ title: 'Error', message: 'Failed to load assignees.' });
        }
      });
    },
    error: () => {
      this.toastServices.show({ title: 'Error', message: 'Failed to get company info.' });
    }
  });
}

assignToUser(userId: number) {
  if (!this.taskD) return;

  this.taskapi.assignTaskToUser(this.taskD.id, userId).subscribe({
    next: () => {
      this.toastServices.show({ title: 'Success', message: 'Task assigned successfully.' });
      this.selectedAssigneeId = userId;
      if(this.taskD)
      this.taskD.assignedUserId = userId;
      // Ayrıca atanan kullanıcı adını güncellemek için istekte bulunabilirsiniz
      this.getAssigneeName(userId);
    },
    error: (err) => {
      this.toastServices.show({ title: 'Error', message: 'Assignment failed: ' + err.message });
    }
  });
}

getAssigneeName(userId: number) {
  this.taskapi.getReporter(userId).subscribe({
    next: (res) => {
      this.assignedUserName = `${res.data.firstName} ${res.data.lastName}`;
    }
  });
}


  getReporter(){
    this.taskapi.getReporter(Number(this.taskD?.creatorId)).subscribe({
      next:(response)=>{
        console.log("fndfnd");

        console.log("fndfnd"+response);
          this.reporterName=`${response.data.firstName} ${response.data.lastName}`
        }
    })
  }

  getAsignee(){
 this.authapi.whoami().subscribe({
  next:(res)=>{
    this.taskapi.getReporter(Number(this.taskD?.assignedUserId)).subscribe({
      next:(response)=>{
        
          this.assignedUserName=`${response.data.firstName} ${response.data.lastName}`
      }
    })
  }
  });
  }



assignTaskToMe() {
  this.authapi.whoami().subscribe({
    next: (response) => {
      const userId = response.userId;
      this.assignedUserName = `${response.firstName} ${response.lastName}`;

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

closeModal(){
    this.taskDeactive.emit(true);
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


loadComments(taskId: number): void {
  this.commentService.getCommentByTaskId(taskId).subscribe({
    next: (res) => {
      this.comments = (res.data ?? []).sort((a, b) => {
        return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
      });
    },
    error: (err) => {
      console.warn('Comment fetch failed', err);
    }
  });
}


submitComment(): void {
  if (!this.newCommentTitle.trim() || !this.newCommentContent.trim() || !this.taskD?.id) {
    this.toastServices.show({
      title: 'Warning',
      message: 'Please enter title and content.'
    });
    return;
  }

  const model: CreateComment = {
    title: this.newCommentTitle.trim(),
    content: this.newCommentContent.trim(),
    taskId: this.taskD.id
  };

  this.commentService.createComment(model).subscribe({
    next: (res) => {
      this.toastServices.show({
        title: 'Success',
        message: 'Comment added successfully.'
      });

    

        this.comments.unshift({
         
           
          id: (res.data && (res.data as any).id) ?? Date.now(), 
          title: model.title,
          content: model.content,
          taskId: model.taskId,
          createdAt: new Date(),
          updateAt: new Date(),
          isActive: true
          
        });
      
    

      this.newCommentTitle = '';
      this.newCommentContent = '';
    },
    error: (err) => {
      this.toastServices.show({
        title: 'Error',
        message: 'Failed to add comment.'
      });
    }
  });
}
commentToDelete: number | null = null;

prepareDelete(commentId: number): void {
  this.commentToDelete = commentId;
}

confirmDeleteComment(): void {
  if (this.commentToDelete === null) return;

  this.commentService.deleteComment(this.commentToDelete).subscribe({
    next: () => {
      this.comments = this.comments.filter(c => c.id !== this.commentToDelete);
      this.toastServices.show({
        title: 'Success',
        message: 'Comment deleted successfully.'
      });
      this.commentToDelete = null;
    },
    error: (err) => {
      this.toastServices.show({
        title: 'Error',
        message: 'Failed to delete comment.'
      });
      this.commentToDelete = null;
    }
  });
}

cancelDelete(): void {
  this.commentToDelete = null;
}

}
