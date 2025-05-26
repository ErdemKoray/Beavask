import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { TaskPriority } from '../../../common/model/taskPriority.model';
import { TaskStatus } from '../../../common/model/taskStatus.model';
import { Subscription } from 'rxjs';
import { CreateTaskModel } from '../../../common/services/task/taskModel/createTask.model';
import { Task } from '../../../common/services/task/taskModel/task.model';
import { ProjectsService } from '../../../common/services/projects/projects.service';
import { TaskService } from '../../../common/services/task/task.service';
import { ActivatedRoute } from '@angular/router';
import { ToastService } from '../../../components/toast/toast.service';
import { CommonModule, DatePipe } from '@angular/common';
import { FormBuilder, FormControl, FormGroup, FormsModule, NgModel, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaskDetailComponent } from '../../home/projects/project-details/project-detail/board/task-detail/task-detail.component';
import { CompanyProfileService } from '../../../common/services/company/profile-company/company-profile.service';
import { MailService } from '../../../common/services/company/mail.service';
import { CompanyService } from '../../../common/services/company/company.service';
import { CompanyUser } from '../../../common/services/company/profile-company/company.model';
import { CompanyMail } from '../../../common/services/company/profile-company/model/company-mail.model';

@Component({
  selector: 'app-company-detail',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule,TaskDetailComponent,FormsModule],
  templateUrl: './company-detail.component.html',
  styleUrl: './company-detail.component.css'
})
export class CompanyDetailComponent implements OnInit, OnDestroy {
   Object = Object;
  companyId!: number;
    projectName = ''; 
 users: CompanyUser[] = [];
  projectId: number = 0;
  taskdetailmodalid = 0;
  isCreateTaskOpen: boolean = false;
  activeDetail = false;
selectedTask: Task | null = null;
companyName='';
  isInviteModalOpen = false;
  inviteForm!: FormGroup;
  selectedUserForInvite?: CompanyUser; // Davet için seçilen kullanıcı
  taskPriorities = Object.values(TaskPriority).filter(v => typeof v === 'number') as number[];
  taskStatus = Object.values(TaskStatus).filter(v => typeof v !== 'number');

  private routeSub: Subscription = new Subscription();

  taskModel: CreateTaskModel = {
    title: '',
    description: '',
    startDate: new Date(),
    dueDate: new Date(),
    priority: 0,
    status: 0,
    projectId: 0,
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

  constructor(
    private apiProject: ProjectsService,
    private apiTask: TaskService,
    private route: ActivatedRoute,
    private toastServices: ToastService,
    private companyProfileService:CompanyProfileService,
    private mailService: MailService,
    private companyService:CompanyService ,
    private fb:FormBuilder   

  ) {}

ngOnInit(): void {
  this.routeSub = this.route.params.subscribe(params => {
    this.projectId = +params['id']; 
    this.companyProfileService.whoamiCompany().subscribe({
      next: (company) => {
        this.companyId = company.companyId; 
        this.companyName=company.companyName
        this.loadProjectAndTasks(this.companyId, this.projectId);
        this.loadCompanyUsers()
      },
      error: () => {
        this.toastServices.show({ title: 'Error', message: 'Failed to get company info.' });
      }
    });
  });
  this.inviteForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
}


  ngOnDestroy(): void {
    this.routeSub.unsubscribe();
  }
   loadCompanyUsers(): void {
    console.log(this.projectId)
    this.companyService.getCompanyUsers(this.projectId).subscribe({
      next: (res) => {
        console.log(res)
        console.log(res)
        this.users = res || [];
        
      },
      error: () => {
        this.toastServices.show({ title: 'Error', message: 'Failed to load users.' });
      }
    });
  }

   canSendInvite(user: CompanyUser): boolean {
    return !(user.isRegistered && user.isAssignedToCompany);
  }

  
sendInviteMail(user: CompanyUser): void {
  if (!user.username) {
    this.toastServices.show({ title: 'Warning', message: 'User does not have a username.' });
    return;
  }

  const mailPayload = {
    toEmail: user.username,  // Artık username atanıyor
    companyName: this.companyName, 
    projectName: this.projectName
  };

  this.mailService.sendMail(mailPayload).subscribe({
    next: () => {
      this.toastServices.show({ title: 'Success', message: `Invite mail sent to user ${user.username}` });
    },
    error: () => {
      this.toastServices.show({ title: 'Error', message: `Failed to send invite mail to user ${user.username}` });
    }
  });
}
 openInviteModal(user: CompanyUser, projectName: string): void {
    this.selectedUserForInvite = user;
    this.projectName = projectName;
    this.inviteForm.patchValue({ email: user.email || '' });
    this.isInviteModalOpen = true;
  }

  closeInviteModal(event: MouseEvent): void {
    this.isInviteModalOpen = false;
    this.inviteForm.reset();
  }

  submitInvite(): void {
    if (this.inviteForm.invalid) return;

    const email = this.inviteForm.value.email.trim();

    if (!email) {
      this.toastServices.show({ title: 'Warning', message: 'Email is required.' });
      return;
    }

    const mailPayload:CompanyMail = {
      toEmail: email,
      companyName: this.companyName, // Dinamik atayabilirsiniz
      projectName: this.projectName
    };
    console.log(mailPayload)
    this.mailService.sendMail(mailPayload).subscribe({
      next: () => {
        this.toastServices.show({ title: 'Success', message: `Invite mail sent to ${email}` });
        this.isInviteModalOpen = false;
        this.inviteForm.reset();
      },
      error: () => {
        this.toastServices.show({ title: 'Error', message: `Failed to send invite mail to ${email}` });
      }
    });
  }

  loadProjectAndTasks(companyId: number, projectId: number): void {
    if (!companyId || !projectId) return;

    this.apiTask.getAllTasks(projectId).subscribe({
      next: (res) => {
        this.tasks = this.formatTasks(res.data);
        this.groupTasksByStatus();
      },
      error: () => {
        this.toastServices.show({ title: 'Error', message: 'Task fetch failed.' });
      }
    });
  }

formatTasks(tasks: any[]): Task[] {
  return tasks.map(task => ({
    ...task,
    createdAt: new Date(task.createdAt),  // mutlaka Date objesi olmalı
    updatedAt: task.updatedAt ? new Date(task.updatedAt) : null,
    startDate: new Date(task.startDate),
    dueDate: new Date(task.dueDate),
    completedDate: task.completedDate ? new Date(task.completedDate) : null
  }));
}


  groupTasksByStatus() {
    Object.keys(this.column).forEach(status => (this.column[status] = []));
    this.tasks.forEach(task => {
      switch (task.status) {
        case 0: this.column['NotStarted'].push(task); break;
        case 1: this.column['InProgress'].push(task); break;
        case 2: this.column['Blocked'].push(task); break;
        case 3: this.column['OnHold'].push(task); break;
        case 4: this.column['Cancelled'].push(task); break;
        case 5: this.column['Completed'].push(task); break;
      }
    });
  }

toggleTaskDetail(taskId: number) {
  this.taskdetailmodalid = taskId;
  this.selectedTask = this.tasks.find(t => t.id === taskId) || null;
  this.activeDetail = !!this.selectedTask;
}

  onTaskUpdated(updatedTask: Task) {
    const index = this.tasks.findIndex(t => t.id === updatedTask.id);
    if (index !== -1) this.tasks[index] = updatedTask;
    this.groupTasksByStatus();
    if (this.taskdetailmodalid === updatedTask.id) this.taskdetailmodalid = updatedTask.id;
  }

  onTaskDeleted(deletedTaskId: number) {
    this.tasks = this.tasks.filter(t => t.id !== deletedTaskId);
    this.groupTasksByStatus();
    this.activeDetail = false;
  }

onCreateTask() {
  if (!this.taskModel.title || !this.taskModel.description) {
    this.toastServices.show({ title: 'Error', message: 'Title and Description are required.' });
    return;
  }

  this.taskModel.projectId = this.projectId;

  const taskData: CreateTaskModel = {
    ...this.taskModel,
    startDate: new Date(this.taskModel.startDate),
    dueDate: new Date(this.taskModel.dueDate)
  };

  this.apiTask.create(taskData).subscribe({
    next: () => {
      this.toastServices.show({ title: 'Success', message: 'Task created successfully!' });
      this.resetForm();
      this.toggleCreateTaskBoard();
      this.loadProjectAndTasks(this.companyId, this.projectId);
    },
    error: (err) => {
      this.toastServices.show({ title: 'Error', message: 'Error creating task: ' + err.message });
    }
  });
}


  resetForm() {
    this.taskModel = {
      title: '',
      description: '',
      startDate: new Date(),
      dueDate: new Date(),
      priority: 0,
      status: 0,
      projectId: this.projectId,
      assignedUserId: 0
    };
  }

  toggleCreateTaskBoard() {
    this.isCreateTaskOpen = !this.isCreateTaskOpen;
  }

  @HostListener('document:keydown.escape', ['$event'])
  onEscape(event: KeyboardEvent) {
    this.isCreateTaskOpen = false;
    this.activeDetail = false;
  }

  @HostListener('document:click', ['$event'])
  handleClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;

    if (this.isCreateTaskOpen && !target.closest('.bv-cp-container') && !target.closest('[data-modal="task"]')) {
      this.isCreateTaskOpen = false;
    }
    if (this.activeDetail && !target.closest('.task-modal') && !target.closest('[data-modal="detail"]')) {
      this.activeDetail = false;
    }
  }
}