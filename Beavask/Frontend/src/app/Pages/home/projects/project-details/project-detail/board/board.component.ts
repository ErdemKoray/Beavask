import { CommonModule, TitleCasePipe } from '@angular/common';
import { AfterViewChecked, AfterViewInit, Component, HostListener, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InlineEditComponent } from './inline-edit/inline-edit.component';
import { TaskDetailComponent } from './task-detail/task-detail.component';
import { ShareModalComponent } from './share/share.component';
import { ProjectsService } from '../../../../../../common/services/projects/projects.service';
import { ActivatedRoute } from '@angular/router';
import { TaskService } from '../../../../../../common/services/task/task.service';
import { CreateTaskModel } from '../../../../../../common/services/task/taskModel/createTask.model';
import { ToastService } from '../../../../../../components/toast/toast.service';
import { TaskPriority ,mapPriority} from '../../../../../../common/model/taskPriority.model';
import { Task } from '../../../../../../common/model/task.model';
import { TaskStatus ,mapStatus} from '../../../../../../common/model/taskStatus.model';




@Component({
  selector: 'app-board',
  standalone: true,
  imports: [CommonModule,FormsModule,TaskDetailComponent,ShareModalComponent,TitleCasePipe],
  templateUrl: './board.component.html',
  styleUrl: './board.component.css'
})
export class BoardComponent implements AfterViewInit {


  projectId: number= 0;
  taskdetailmodalid=0;
  isCreateTaskOpen: boolean = false;
  shareVisible = false;
  currentTaskUrl = 'https://example.com/task/CCS-3';
  isCreateProjectOpen = false;
  activeDetail=false
  taskPriorities = Object.values(TaskPriority).filter(value => typeof value === 'number') as number[];
  taskStatus = Object.values(TaskPriority).filter(value => typeof value !== 'number'); 
  
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
    Blocked: [],
    OnHold: [],
    Cancelled: [],
    Completed: []
  };

 
  constructor(private apiProject:ProjectsService,
    private apiTask:TaskService,
    private route:ActivatedRoute,
    private toastServices:ToastService) { }

  ngAfterViewInit() {
 
    this.getProjectDetail();
    this.getProjectTasks()
  }





  
  
  
  




//GENEL FONKSİYONLAR BÖLÜMÜ
showShare() { this.shareVisible = true; }
  toggleCreateProjectBoard() {
    this.isCreateProjectOpen = !this.isCreateProjectOpen;
    
  }
  
  toggleTaskDetail(id:number) {
    this.activeDetail = !this.activeDetail;
    this.taskdetailmodalid=id;
    console.log(this.taskdetailmodalid)
    
  }



  //API BÖLÜMÜ

  getProjectDetail() {
    this.route.params.subscribe(params => {
      this.projectId = params['projectId']; 
      console.log(this.projectId)
    }
  );
    this.apiProject.getById(this.projectId).subscribe((response) => {
      if (response?.data) {
       
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
      startDate: task.startDate ? new Date(task.startDate) : null,
      dueDate: task.dueDate ? new Date(task.dueDate) : null,
      completedDate: task.completedDate ? new Date(task.completedDate) : null,
      priority: mapPriority(task.priority), 
      status: mapStatus(task.status),  
      projectId: task.projectId,
      assignedUserId: task.assignedUserId || undefined, 
      assignedUser: task.assignedUser || undefined 
    }));
  }

  
  getStatusString(status: TaskStatus): string {
    return TaskStatus[status]; 
  }

 
  getPriorityString(priority: TaskPriority): string {
    return TaskPriority[priority];
  }

  
  groupTasksByStatus() {
    // Her bir statüye göre görevleri sıfırla (column[status] her zaman boş olmalı)
    Object.keys(this.column).forEach(status => {
      this.column[status] = [];
    });
  
    // Her bir görevi statüsüne göre doğru sütuna yerleştiriyoruz
    this.tasks.forEach(task => {
      // Status'a göre doğru sütuna yerleştiriyoruz
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
          console.log('Unknown status', task.status);
          break;
      }
    });
  
    // Konsola gruplandırılmış görevleri yazdıralım
    console.log(this.column); // Gruplandırılmış görevleri kontrol ediyoruz
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
         },
         error:(err) => {
           this.toastServices.show(
             {title:'error',message:'Error creating task!:' + err.message,
   
             });
         }}
       );
    } else {
      // Eğer başlık veya açıklama boşsa kullanıcıyı bilgilendirebiliriz
      alert("Title and Description are required.");
    }
  }

  // Task formunu sıfırlama fonksiyonu
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

  // Modal açma/kapama fonksiyonu
  toggleCreateTaskBoard() {
    this.isCreateTaskOpen = !this.isCreateTaskOpen;
    this.resetForm(); 
  }
  //API İLE İLGİLİ FONKSİYONLAR BÖLÜMÜ


  //HOST LISTENERS BÖLÜMÜ
  @HostListener('document:keydown.escape', ['$event'])
  onEscape(event: KeyboardEvent) {
    this.isCreateProjectOpen = false;
    this.activeDetail=false;
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
  }
  

}


