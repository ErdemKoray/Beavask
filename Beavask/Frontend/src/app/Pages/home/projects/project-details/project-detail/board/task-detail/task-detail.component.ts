import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { Task } from '../../../../../../../common/services/task/taskModel/task.model';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskService } from '../../../../../../../common/services/task/task.service';
import { AuthprofileService } from '../../../../../../../common/services/profile/authprofile.service';
import { ToastService } from '../../../../../../../components/toast/toast.service';
import { UpdateTaskModel } from '../../../../../../../common/services/task/taskModel/updateTask.model';
import { CommentService } from '../../../../../../../common/services/comment/comment.service';
import { Comment } from '../../../../../../../common/model/comment/comment.model';
import { CompanyService } from '../../../../../../../common/services/company/company.service';
import { AuthCompanyService } from '../../../../../../../common/services/company/auth-company.service';
import { CreateComment } from '../../../../../../../common/model/comment/createcomment.model';
import { ProjectUserService } from '../../../../../../../common/services/projects/project-user.service';

@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe],
  templateUrl: './task-detail.component.html',
  styleUrls: ['./task-detail.component.css']
})
export class TaskDetailComponent implements OnInit {
  @Output() taskUpdated = new EventEmitter<Task>();
  @Output() taskDeleted = new EventEmitter<number>();
  @Output() taskDeactive = new EventEmitter<boolean>();

  @Input() taskD: Task | null = null;
  assignees: { userId: number; fullName: string }[] = [];
  assignedUserId: number | null = null;
  selectedAssigneeId?: number;
  comments: Comment[] = [];
  newCommentTitle: string = '';
  newCommentContent: string = '';
  isEditingDescription = false;
  updatedDescription: string = '';
  reporterName: string = '';
  assignedUserName: string = '';
  isAssigned: boolean = false;

  statusList = [
    { label: 'Not Started', value: 0 },
    { label: 'In Progress', value: 1 },
    { label: 'Blocked', value: 2 },
    { label: 'On Hold', value: 3 },
    { label: 'Cancelled', value: 4 },
    { label: 'Completed', value: 5 }
  ];

  constructor(
    private taskapi: TaskService,
    private authapi: AuthprofileService,
    private toastServices: ToastService,
    private commentService: CommentService,
    private companyService: CompanyService,
    private authCompanyService: AuthCompanyService,
    private projectUserService:ProjectUserService
  ) {}

  ngOnInit() {
    if (this.taskD) {
      this.loadAssignees();
      this.updatedDescription = this.taskD.description || '';
    
      this.getReporterName(Number(this.taskD.creatorId))
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
        if(company.companyName!=""&& company.companyName!=null){
          this.reporterName = company.companyName;
          this.companyService.getCompanyUsers(Number(this.taskD?.projectId)).subscribe({
            next: (res) => {
              this.assignees = res.map((u) => ({
                userId: u.id,
                fullName: `${u.username}`.trim()
              }));
              if (this.taskD?.assignedUserId) this.selectedAssigneeId = this.taskD.assignedUserId;
              this.getAssigneeName(Number(this.selectedAssigneeId))
            },
            error: () => {}
          });
        }else{
          this.projectUserService.getProjectUser(Number(this.taskD?.projectId)).subscribe({
            next:(res)=>{
              this.assignees = res.data.map((u) => ({
                userId: u.id,
                fullName: `${u.username}`.trim()
              }))
               if (this.taskD?.assignedUserId) this.selectedAssigneeId = this.taskD.assignedUserId;
                this.getAssigneeName(Number(this.selectedAssigneeId))
            }
          })
        
        }
      },
      error: () => {}
    });
  }

  // Görev ataması yapma
  assignToUser(userId: number) {
    if (!this.taskD) return;
    this.taskapi.assignTaskToUser(this.taskD.id, userId).subscribe({
      next: () => {
        this.toastServices.show({ title: 'Success', message: 'Task assigned successfully.' });
        this.selectedAssigneeId = userId;
        if (this.taskD) this.taskD.assignedUserId = userId;
        this.getAssigneeName(userId);
      },
      error: (err) => {}
    });
  }

  // Görevli ismini al
  getAssigneeName(userId: number) {
    this.taskapi.getReporter(userId).subscribe({
      next: (res) => {
        this.assignedUserName = `${res.data.username}`;
      }
    });
  }
    getReporterName(userId: number) {
      console.log(userId)
    this.taskapi.getReporter(userId).subscribe({
      next: (res) => {
        this.reporterName = `${res.data.username}`;
      }
    });
  }

  // Görev açıklaması kaydetme
async saveDescription() {
  if (!this.taskD) return;

  try {
    // Kullanıcı bilgisini al
    const response = await this.authapi.whoami().toPromise();
    const currentUserId = response?.userId;

    // Yetki kontrolü: Görevi yalnızca atanan kullanıcı güncelleyebilir
    if (this.taskD!.assignedUserId !== currentUserId) {
      this.toastServices.show({
        title: 'Unauthorized',
        message: 'You are not allowed to update this task.'
      });
      this.isEditingDescription = false;
      return;
    }

    // Güncellenmiş modeli oluştur
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

    // Görev güncellemesi yap
    await this.taskapi.updateTask(this.taskD.id, updateModel).toPromise();

    // Başarılı güncelleme sonrası işlem
    this.toastServices.show({
      title: 'Success',
      message: 'Description updated successfully.'
    });
    this.taskD!.description = this.updatedDescription;
    this.taskUpdated.emit(this.taskD!);
    this.isEditingDescription = false;

  } catch (err) {
    this.toastServices.show({
      title: 'Error',
      message: 'Update failed: ' 
    });
  }
}


  cancelDescriptionEdit(event: MouseEvent) {
    event.stopPropagation();
    this.updatedDescription = this.taskD?.description || '';
    this.isEditingDescription = false;
  }

async updateStatus(newStatus: number) {
  if (!this.taskD || this.taskD.status === newStatus) return;

  try {
    const response = await this.authapi.whoami().toPromise();  
    const currentUserId = response?.userId;

    if (this.taskD.assignedUserId !== currentUserId) {
    
      this.toastServices.show({
        title: 'Unauthorized',
        message: 'Only the assigned user can change the status.'
      });
      return;
    }

    const updateModel: UpdateTaskModel = {
      title: this.taskD.title,
      description: this.taskD.description,
      startDate: this.taskD.startDate.toISOString(),
      dueDate: this.taskD.dueDate.toISOString(),
      completedDate: this.taskD.completedDate?.toISOString() ?? new Date().toISOString(),
      updatedAt: new Date().toISOString(),
      priority: this.taskD.priority,
      status: Number(newStatus),  // Durum güncelleniyor
      assignedUserId: this.taskD.assignedUserId ?? 0
    };

    await this.taskapi.updateTask(this.taskD.id, updateModel).toPromise();

    this.taskD.status = newStatus;
    this.taskUpdated.emit(this.taskD);
    this.toastServices.show({ title: 'Success', message: 'Status updated.' });
  } catch (err) {
    this.toastServices.show({
      title: 'Error',
      message: 'Status update failed: '
    });
  }
}



  deleteTask() {
    if (!this.taskD) return;
    this.taskapi.deleteTask(this.taskD.id).subscribe({
      next: () => {
        this.toastServices.show({ title: 'Success', message: 'Task deleted successfully.' });
        this.taskDeleted.emit(this.taskD?.id);
      },
      error: (err) => {
        this.toastServices.show({ title: 'Error', message: 'Deletion failed: ' + err.message });
      }
    });
  }


  loadComments(taskId: number): void {
    this.commentService.getCommentByTaskId(taskId).subscribe({
      next: (res) => {
        this.comments = res.data?.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()) ?? [];
      },
      error: (err) => {
        console.warn('Comment fetch failed', err);
      }
    });
  }


  submitComment(): void {
    if (!this.newCommentTitle.trim() || !this.newCommentContent.trim() || !this.taskD?.id) {
      this.toastServices.show({ title: 'Warning', message: 'Please enter title and content.' });
      return;
    }
    const model: CreateComment = {
      title: this.newCommentTitle.trim(),
      content: this.newCommentContent.trim(),
      taskId: this.taskD.id
    };
    this.commentService.createComment(model).subscribe({
      next: (res) => {
        this.toastServices.show({ title: 'Success', message: 'Comment added successfully.' });
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
        this.toastServices.show({ title: 'Error', message: 'Failed to add comment.' });
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
        this.comments = this.comments.filter((c) => c.id !== this.commentToDelete);
        this.toastServices.show({ title: 'Success', message: 'Comment deleted successfully.' });
        this.commentToDelete = null;
      },
      error: (err) => {
        this.toastServices.show({ title: 'Error', message: 'Failed to delete comment.' });
        this.commentToDelete = null;
      }
    });
  }

  cancelDelete(): void {
    this.commentToDelete = null;
  }
}
