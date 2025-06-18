import { CommonModule, TitleCasePipe } from '@angular/common';
import {  Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { TaskDetailComponent } from './task-detail/task-detail.component';
import { ShareModalComponent } from './share/share.component';
import { ProjectsService } from '../../../../../../common/services/projects/projects.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { TaskService } from '../../../../../../common/services/task/task.service';
import { CreateTaskModel } from '../../../../../../common/services/task/taskModel/createTask.model';
import { ToastService } from '../../../../../../components/toast/toast.service';
import { TaskPriority } from '../../../../../../common/model/taskPriority.model';
import { Task } from '../../../../../../common/services/task/taskModel/task.model';
import { TaskStatus } from '../../../../../../common/model/taskStatus.model';
import { Subscription } from 'rxjs';
import { PnavbarComponent } from '../pnavbar/pnavbar.component';
import { SummaryComponent } from "../summary/summary.component";
import {
  CdkDragDrop,
  CdkDrag,
  CdkDropList,
  CdkDropListGroup,
  moveItemInArray,
  transferArrayItem,
} from '@angular/cdk/drag-drop';
import { UpdateTaskModel } from '../../../../../../common/services/task/taskModel/updateTask.model';
import { AuthprofileService } from '../../../../../../common/services/profile/authprofile.service';
import { ProjectDetail } from '../../../../../../common/services/projects/ProjectDetail.model';
import { Project } from '../../../../../../common/model/project.model';
import { TranslateModule } from '@ngx-translate/core';
import { ProjectUserService } from '../../../../../../common/services/projects/project-user.service';
import { Friend } from '../../../../../../common/services/friendship/friendship.service';




@Component({
  selector: 'app-board',
  standalone: true,
  imports: [CommonModule,
        TranslateModule,
     PnavbarComponent,
      FormsModule,
       TaskDetailComponent, 
       ShareModalComponent,
        TitleCasePipe,
         SummaryComponent,
         CdkDropListGroup, 
         CdkDropList,
          CdkDrag,
          RouterLink
        ],
  templateUrl: './board.component.html',
  styleUrl: './board.component.css'
})
export class BoardComponent implements OnInit,OnDestroy {
 searchQuery: string = ''; 
 
  activeBoard=true;
  activeSummary=false;

  projectId: number= 0;
  projectDetail: Project | null = null;
  taskdetailmodalid = 0;
  isCreateTaskOpen: boolean = false;
  shareVisible = false;
  taskDetail: Task | null = null;
  currentTaskUrl = 'https://example.com/task/CCS-3';
  isCreateProjectOpen = false;
  activeDetail=false
  taskPriorities = Object.values(TaskPriority).filter(value => typeof value === 'number') as number[];
private routeSub: Subscription = new Subscription();
  
  taskModel: CreateTaskModel = {
    title: '',
    description: '',
    startDate: new Date(),
    dueDate: new Date(),
    priority: 0,
    status: 0,
    projectId: this.projectId,
    assignedUserId: 0
  };

  tasks: Task[] = [];
  column: { [key: string]: Task[] } = {
    NotStarted: [],
    InProgress: [],
    OnHold: [],
    Cancelled: [],
    Completed: []
  };

   friends: Friend[] = [];
  constructor(private apiProject:ProjectsService,
    private apiTask:TaskService,
    private route:ActivatedRoute,
    private toastServices:ToastService,
  private authapi:AuthprofileService,
  private projectUserService:ProjectUserService
) { }

ngOnInit(): void {
  this.routeSub = this.route.params.subscribe(params => {
    this.projectId = +params['projectId'];
    this.getProjectDetail(this.projectId);

    this.route.queryParams.subscribe(query => {
      const incomingTaskId = query['TaskId'] ? +query['TaskId'] : null;

      this.apiTask.getAllTasks(this.projectId).subscribe({
        next: (res) => {
          this.tasks = this.formatTasks(res.data);
          this.groupTasksByStatus();
          this.projectUser();

          if (incomingTaskId) {
            this.toggleTaskDetail(incomingTaskId);
          }
        },
        error: (err) => {

        }
      });
    });
  });
}


onImageError(event: Event) {
  const imgElement = event.target as HTMLImageElement;
  imgElement.src = 'iconbeavask.png'; 
}
  ngOnDestroy(): void {
    this.routeSub.unsubscribe()
  }


projectUser(){
          this.projectUserService.getProjectUser(this.projectId).subscribe({
          next: (response) => {
            console.log(response.data)
            this.friends = response.data || [];       
            
          },
          error: () => { 
         
          }
        });
}

//GENEL FONKSİYONLAR BÖLÜMÜ

handleInvite(userId: number) {
  // Örn: snackbar, listeyi güncelle vs.
  console.log('Davet gönderildi:', userId);
}

onActiveBoardChange(active: boolean) {
  this.activeBoard = active;
}

showShare() { this.shareVisible = true; }
  toggleCreateProjectBoard() {
    this.isCreateProjectOpen = !this.isCreateProjectOpen;
    
  }
  
 toggleTaskDetail(taskId: number) {
    this.taskdetailmodalid = taskId;
    this.activeDetail = !this.activeDetail;

 
    this.taskDetail = this.tasks.find(task => task.id === taskId) || null;

  }

onTaskUpdated(updatedTask: Task) {
  const index = this.tasks.findIndex(t => t.id === updatedTask.id);
  if (index !== -1) {
    this.tasks[index] = updatedTask;
  }

  this.groupTasksByStatus();

  if (this.taskDetail?.id === updatedTask.id) {
    this.taskDetail = { ...updatedTask }; 
  }
}
onTaskDeleted(deletedTaskId: number) {
  // 1. Task listesinden sil
  this.tasks = this.tasks.filter(t => t.id !== deletedTaskId);

  // 2. Sütunları güncelle
  this.groupTasksByStatus();

  // 3. Modalı kapat
  this.activeDetail = false;
  this.taskDetail = null;
}

  //API BÖLÜMÜ

  getProjectDetail(pId:number) {
 
    this.apiProject.getById(pId).subscribe((response) => {
      if (response?.data) {
       console.log('Project Details:', response.data);
       this.projectDetail = response.data;
      }
    });
  }

  
  getProjectTasks() {
    this.projectId = Number(this.projectId); 
    this.apiTask.getAllTasks(this.projectId).subscribe(
      {
        next: (res) => {
          this.tasks = this.formatTasks(res.data); 
          this.groupTasksByStatus();
          
        },
        error: (err) => {
       
        }
      }
    );
  
  }

formatTasks(tasks: any[]): Task[] {
  return tasks.map(task => ({
    id: task.id,
    title: task.title,
    description: task.description,
    createdAt: new Date(task.createdAt),
    updatedAt: task.updatedAt ? new Date(task.updatedAt) : null,
    startDate: new Date(task.startDate),
    dueDate: new Date(task.dueDate),
    completedDate: task.completedDate ? new Date(task.completedDate) : null,
    isActive: true,
    priority: task.priority,
    status: task.status,
    projectId: task.projectId,
    project: task.project || null,
    creatorId: task.creatorId ?? null,
    assignedUserId: task.assignedUserId ?? null,
    assignedUser: task.assignedUser || null
  }));
}


  
  getStatusString(status: TaskStatus): string {
    return TaskStatus[status]; 
  }

 
  getPriorityString(priority: TaskPriority): string {
    return TaskPriority[priority];
  }

  
  groupTasksByStatus() {
    Object.keys(this.column).forEach(status => {
      this.column[status] = [];
    });
    this.tasks.forEach(task => {

      switch (task.status) {
        case 0:
          this.column['NotStarted'].push(task);
          break;
        case 1:
          this.column['InProgress'].push(task);
          break;
        case 2:
          this.column['Blocked'].push(task);
          break;
        case 3:
          this.column['OnHold'].push(task);
          break;
        case 4:
          this.column['Cancelled'].push(task);
          break;
        case 5:
          this.column['Completed'].push(task);
          break;
        default:
          break;
      }
    });
  }
  



  onCreateTask() {
    if (this.taskModel.title && this.taskModel.description) {
      this.taskModel.projectId = Number(this.projectId); 
      this.taskModel.priority = Number(this.taskModel.priority);
      this.taskModel.status = Number(this.taskModel.status);
      this.taskModel.startDate= new Date(this.taskModel.startDate);
      this.taskModel.dueDate= new Date(this.taskModel.dueDate);
     
      this.apiTask.create(this.taskModel).subscribe(
        { next:(res) => {
           this.toastServices.show(
             {title:'success',message:'Task created successfully!',
   
             });
           this.resetForm();
           this.toggleCreateTaskBoard();
           this.getProjectTasks()
          
         },
         error:(err) => {
           this.toastServices.show(
             {title:'error',message:'Error creating task!:' + err.message,
   
             });
         }}
       );
    } else {
      this.toastServices.show(
             {title:'error',message:'Title and Description are required.',
   
             });
    }
  }

  resetForm() {
    this.taskModel = {
      title: '',
      description: '',
      startDate: new Date(),
      dueDate: new Date(),
      priority: 0,
      status: 0,
      projectId: 0,
      assignedUserId: 0
    };
  }

  toggleCreateTaskBoard() {
    this.isCreateTaskOpen = !this.isCreateTaskOpen;
    this.resetForm(); 
  }


  //HOST LISTENERS BÖLÜMÜ
  @HostListener('document:keydown.escape', ['$event'])
  onEscape(event: KeyboardEvent) {
    this.isCreateProjectOpen = false;
    this.activeDetail=false;
    this.isCreateTaskOpen=false;
    this.shareVisible=false;
  }
  @HostListener('document:click', ['$event'])
  handleClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;
  

    if (
      this.isCreateProjectOpen &&
      !target.closest('.bv-cp-container') &&
      !target.closest('[data-dropdown="create"]')
    ) {
      this.isCreateProjectOpen = false;
    }

    if (
      this.activeDetail &&
      !target.closest('.task-modal') &&
      !target.closest('[data-modal="detail"]')
    ) {
      this.activeDetail = false;
    }
      
     if (
      this.shareVisible &&
      !target.closest('.project-invite-root') &&
      !target.closest('[data-modal="shareVisible"]')
    ) {
      this.shareVisible = false;
    }
  }
  

taskStatusKeys = Object.keys(this.column); 


drop(event: CdkDragDrop<Task[]>) {
  if (event.previousContainer === event.container) {
    moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
  } else {
    const task = event.previousContainer.data[event.previousIndex];
    const newStatusEnum = this.mapStatusStringToEnum(event.container.id);

    // Status aynıysa işlem yapma
    if (task.status === newStatusEnum) {
      return;
    }

    this.authapi.whoami().subscribe({
      next: (response) => {
        const currentUserId = response.userId;
        if (task.assignedUserId !== currentUserId) {
          this.toastServices.show({
            title: 'Unauthorized',
            message: 'Only the assigned user can change the status.'
          });
          return;
        }

        const updatedTaskModel: UpdateTaskModel = {
          title: task.title,
          description: task.description,
          startDate: task.startDate.toISOString(),
          dueDate: task.dueDate.toISOString(),
          completedDate: task.completedDate ? task.completedDate.toISOString() : new Date().toISOString(),
          updatedAt: new Date().toISOString(),
          priority: task.priority,
          status: newStatusEnum,
          assignedUserId: task.assignedUserId ?? 0
        };

        this.apiTask.updateTask(task.id, updatedTaskModel).subscribe({
          next: () => {
            task.status = newStatusEnum;
            task.updatedAt = new Date();
            transferArrayItem(
              event.previousContainer.data,
              event.container.data,
              event.previousIndex,
              event.currentIndex
            );

          },
          error: err => {
            this.toastServices.show({
              title: 'Error',
              message: 'Status update failed: ' + (err?.error?.message || err.message)
            });
          }
        });
      },
      error: err => {
        this.toastServices.show({
          title: 'Error',
          message: 'Authorization check failed: ' + (err?.error?.message || err.message)
        });
        
      }
    });
  }
}


mapStatusStringToEnum(statusString: string): TaskStatus {
  switch (statusString) {
    case 'NotStarted': return TaskStatus.NotStarted;
    case 'InProgress': return TaskStatus.InProgress;
    case 'OnHold': return TaskStatus.OnHold;
    case 'Cancelled': return TaskStatus.Cancelled;
    case 'Completed': return TaskStatus.Completed;
    default: return TaskStatus.NotStarted;
  }
}

 getFilteredTasks(status: string): Task[] {
    // 'searchQuery' ile arama yapıyoruz
    const query = this.searchQuery.toLowerCase();
    
    // Eğer arama boşsa tüm taskları döndürüyoruz
    if (!query) {
      return this.column[status];
    }

    // Arama terimine göre taskları filtreliyoruz
    return this.column[status].filter(task => 
      task.title.toLowerCase().includes(query) || 
      task.description.toLowerCase().includes(query)
    );
  }
}


