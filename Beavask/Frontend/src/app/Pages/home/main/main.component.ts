import { CommonModule, DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core'; 
import { Task } from '../../../common/services/task/taskModel/task.model';
import { TaskService } from '../../../common/services/task/task.service';
import { AuthprofileService } from '../../../common/services/profile/authprofile.service';
import { RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { Project } from '../../../common/model/project.model';
import { NumberSequencePipe } from '../../../common/pipe/numberSequence.pipe';
import { ProjectsService } from '../../../common/services/projects/projects.service';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [CommonModule, TranslateModule, DatePipe, RouterLink,NumberSequencePipe],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent implements OnInit  {
  workedActivities: Task[] = [];
  assignedActivities: Task[] = [];
recentProjects: Project[] = [];

  // activeTab türünü kesin belirttik
  activeTab: 'worked' | 'assigned' = 'worked';

  pageSize = 5;

  // currentPage nesnesi activeTab tipine uygun anahtarlar içeriyor
  currentPage: { [key in 'worked' | 'assigned']: number } = { worked: 1, assigned: 1 };

  displayedActivities: Task[] = [];

  constructor(
    private taskService: TaskService,
    private authService: AuthprofileService,
    private projectService:ProjectsService
  ) {}

  ngOnInit(): void {
    this.authService.whoami().subscribe(user => {
      this.getUserProject();
      const userId = user.userId;

      this.taskService.getUserTaskById(userId).subscribe(res => {
        const allTasks = res.data;

        // worked on: status = 1 (in-progress)
        this.workedActivities = allTasks.filter(t => t.status === 1);

       
        this.assignedActivities = allTasks.filter(t => t.assignedUserId === userId);

        this.updateDisplayedActivities();
      });
    });
  }

  // Tab değiştirirken aktif tab türünü belirtmek için güncellendi
  setTab(tab: 'worked' | 'assigned'): void {
    this.activeTab = tab;
    this.updateDisplayedActivities();
  }

  // Aktif tab'a göre listelerden seçim yapıyoruz
  get activeActivitiesList(): Task[] {
    return this.activeTab === 'worked' ? this.workedActivities : this.assignedActivities;
  }

  get totalItems(): number {
    return this.activeActivitiesList.length;
  }

  get totalPages(): number {
    return Math.ceil(this.totalItems / this.pageSize);
  }

  get currentItemsRange(): string {
    if (this.totalItems === 0) return '0-0 / 0';
    const start = (this.currentPage[this.activeTab] - 1) * this.pageSize + 1;
    const end = Math.min(start + this.pageSize - 1, this.totalItems);
    return `${start}-${end} / ${this.totalItems}`;
  }

  get canGoPrev(): boolean {
    return this.currentPage[this.activeTab] > 1;
  }

  get canGoNext(): boolean {
    return this.currentPage[this.activeTab] < this.totalPages;
  }

  updateDisplayedActivities(): void {
    const currentPage = this.currentPage[this.activeTab];
    const startIndex = (currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.displayedActivities = this.activeActivitiesList.slice(startIndex, endIndex);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage[this.activeTab] = page;
      this.updateDisplayedActivities();
    }
  }

  nextPage(): void {
    this.goToPage(this.currentPage[this.activeTab] + 1);
  }

  prevPage(): void {
    this.goToPage(this.currentPage[this.activeTab] - 1);
  }
  getUserProject() {
  this.projectService.getAll().subscribe({
    next: (Response) => {
      console.log(Response.data);
      this.recentProjects = Response.data
        .map((project: any) => ({
          id: project.id,
          name: project.name,
          description: '',
          createdAt: new Date(project.createdAt),
          isActive: true,
          customerId: 0
        }))
        .filter((project, index, self) => {
          return index >= self.length - 3;
        });
    },
    error: (err) => {
      const errorMessage = err?.error?.message || 'An unexpected error occurred';

    },
  });
}
}
