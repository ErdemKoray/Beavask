import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { TaskService } from '../../../common/services/task/task.service';
import { AuthprofileService } from '../../../common/services/profile/authprofile.service';
import { ToastService } from '../../../components/toast/toast.service';
import { Task } from '../../../common/services/task/taskModel/task.model';
import { ProjectsService } from '../../../common/services/projects/projects.service';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-my-task',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,RouterLink,TranslateModule],
  templateUrl: './my-task.component.html',
  styleUrl: './my-task.component.css'
})
export class MyTaskComponent implements OnInit {
  taskFilterForm!: FormGroup;
projectMap: { [key: number]: string } = {};

  projects: string[] = [];
  teams: string[] = [];

  allTasks: Task[] = [];
  filteredTasks: Task[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private taskService: TaskService,
    private authService: AuthprofileService,
    private toastService: ToastService,
    private projectService:ProjectsService
  ) {}

  ngOnInit(): void {
    
    this.taskFilterForm = this.fb.group({
      project: [''],
      team: ['']
    });

    this.authService.whoami().subscribe({
      next: user => {
        const userId = user.userId;

        this.taskService.getUserTaskById(userId).subscribe({
          next: res => {
           const allFetchedTasks = res.data;

this.allTasks = allFetchedTasks
  .filter(task => task.assignedUserId === user.userId)
  .sort((a, b) =>
    new Date(b.updatedAt ?? b.createdAt).getTime() -
    new Date(a.updatedAt ?? a.createdAt).getTime()
  );


            this.allTasks.forEach(task => {
            if (task.projectId) {
              this.getProjectNameById(task.projectId);
            }
          });

            this.projects = [...new Set(this.allTasks.map(task => task.project?.name || ''))];
            this.teams = [...new Set(this.allTasks.map(task => task.assignedUser?.team?.title || ''))];

            this.filteredTasks = [...this.allTasks];

            this.route.queryParams.subscribe(params => {
              const project = params['project'];
              const team = params['team'];
              const status = params['status'];

              this.filteredTasks = this.allTasks.filter(task => {
                const matchesProject = project ? task.project?.name === project : true;
                const matchesTeam = team ? task.assignedUser?.team?.title === team : true;
                const matchesStatus = status ? task.status === +status : true;
                return matchesProject && matchesTeam && matchesStatus;
              });

              this.taskFilterForm.patchValue({
                project: project || '',
                team: team || ''
              });
            });
            console.log(this.projects)

          },
          error: err => {
          }
        });
      },
      error: err => {
        this.toastService.show({ title: 'Error', message: 'User not found: ' + err.message });
      }
    });
  }
getProjectNameById(id: number): void {
  if (this.projectMap[id]) return;

  this.projectService.getById(id).subscribe({
    next: res => {
      const projectName = res.data.name;
      this.projectMap[id] = projectName;

      if (!this.projects.includes(projectName)) {
        this.projects.push(projectName); // ✅ Dropdown için proje adlarını listeye ekle
      }
    },
    error: err => {
      console.warn('Project name fetch failed', err);
      this.projectMap[id] = 'Unknown';
    }
  });
}


 filterTasks(): void {
  const { project, team } = this.taskFilterForm.value;

  this.filteredTasks = this.allTasks.filter(task => {
    const projectName = this.projectMap[task.projectId] || '';
    const teamName = task.assignedUser?.team?.title || '';

    const matchesProject = project ? projectName === project : true;
    const matchesTeam = team ? teamName === team : true;

    return matchesProject && matchesTeam;
  });
}

  getPriorityLabel(priority: number): string {
  switch (priority) {
    case 0: return 'Low';
    case 1: return 'Medium';
    case 2: return 'High';
    case 3: return 'Critical';
    default: return 'Unknown';
  }
}

getPriorityClass(priority: number): string {
  switch (priority) {
    case 0: return 'low';
    case 1: return 'medium';
    case 2: return 'high';
    case 3: return 'critical';
    default: return 'default';
  }
}

getStatusLabel(status: number): string {
  switch (status) {
    case 0: return 'Not Started';
    case 1: return 'In Progress';
    case 2: return 'Blocked';
    case 3: return 'On Hold';
    case 4: return 'Cancelled';
    case 5: return 'Completed';
    default: return 'Unknown';
  }
}

getStatusClass(status: number): string {
  return this.getStatusLabel(status).toLowerCase().replace(/\s/g, '-');
}
pageSize = 5;
currentPage = 1;

get totalPages(): number {
  return Math.ceil(this.filteredTasks.length / this.pageSize);
}

get currentItemsRange(): string {
  if (this.filteredTasks.length === 0) return '0-0 / 0';
  const start = (this.currentPage - 1) * this.pageSize + 1;
  const end = Math.min(start + this.pageSize - 1, this.filteredTasks.length);
  return `${start}-${end} / ${this.filteredTasks.length}`;
}

get paginatedTasks(): Task[] {
  const start = (this.currentPage - 1) * this.pageSize;
  const end = start + this.pageSize;
  return this.filteredTasks.slice(start, end);
}

nextPage(): void {
  if (this.currentPage < this.totalPages) this.currentPage++;
}

prevPage(): void {
  if (this.currentPage > 1) this.currentPage--;
}

goToPage(page: number): void {
  if (page >= 1 && page <= this.totalPages) {
    this.currentPage = page;
  }
}
get pageNumbers(): number[] {
  return Array.from({ length: this.totalPages }, (_, i) => i + 1);
}

}
