import { CommonModule, DatePipe } from '@angular/common';
import { Component, AfterViewInit, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TaskService } from '../../../common/services/task/task.service';
import { ProjectsService } from '../../../common/services/projects/projects.service';
import { AuthprofileService } from '../../../common/services/profile/authprofile.service';
import { Task } from '../../../common/services/task/taskModel/task.model';

import {
  Chart,
  registerables
} from 'chart.js';
import { Project } from '../../../common/model/project.model';
import { CommentService } from '../../../common/services/comment/comment.service';
import { Comment } from '../../../common/model/comment/comment.model';

Chart.register(...registerables);

@Component({
  selector: 'app-my-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,DatePipe],
  templateUrl: './my-dashboard.component.html',
  styleUrl: './my-dashboard.component.css'
})
export class MyDashboardComponent implements OnInit {
  recentTasks: Task[] = [];
  activeProjects: Project[] = [];
  recentComments: Comment[] = [];

  taskStats = { completed: 0, created: 0, inProgress: 0, dueToday: 0 };

  constructor(
    private router: Router,
    private taskService: TaskService,
    private projectService: ProjectsService,
    private authService: AuthprofileService,
    private commentService: CommentService
  ) {}

  ngOnInit(): void {
    this.authService.whoami().subscribe({
      next: user => {
        const userId = user.userId;

        this.taskService.getUserTaskById(userId).subscribe({
          next: res => {
            const tasks = res.data.filter(t =>
              t.assignedUserId === userId || t.creatorId === userId
            );

            const now = new Date();
            const todayStr = now.toISOString().split('T')[0];

            this.recentTasks = tasks
              .sort((a, b) => new Date(b.updatedAt ?? b.createdAt).getTime() - new Date(a.updatedAt ?? a.createdAt).getTime())
              .slice(0, 5);

            this.taskStats.completed = tasks.filter(t => t.status === 5).length;
            this.taskStats.created = tasks.filter(t => {
              const createdDate = new Date(t.createdAt).toISOString().split('T')[0];
              return createdDate >= todayStr;
            }).length;
            this.taskStats.inProgress = tasks.filter(t => t.status === 1).length;
            this.taskStats.dueToday = tasks.filter(t => {
              const dueDate = new Date(t.dueDate).toISOString().split('T')[0];
              return dueDate === todayStr;
            }).length;

            this.createStatusChart(tasks);
          }
        });

        this.projectService.getAll().subscribe({
          next: res => {
            console.log(res.data);
            this.activeProjects = res.data
              .slice(0, 5);
              console.log(this.activeProjects);
          }
        });

        // ✅ Yorumları çek
        this.commentService.getCommentByUserId(userId).subscribe({
          next: res => {
            this.recentComments = res.data
              .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
              .slice(0, 3); // sadece son 3 yorum
          }
        });
      }
    });
  }


  createStatusChart(tasks: Task[]): void {
    const statusCounts = {
      todo: tasks.filter(t => t.status === 0).length,
      inProgress: tasks.filter(t => t.status === 1).length,
      done: tasks.filter(t => t.status === 5).length
    };

    console.log(statusCounts)

    const canvas = document.getElementById('statusChart') as HTMLCanvasElement;
    if (!canvas) return;

    new Chart(canvas, {
      type: 'doughnut',
      data: {
        labels: ['To Do', 'In Progress', 'Done'],
        datasets: [{
          data: [statusCounts.todo, statusCounts.inProgress, statusCounts.done],
          backgroundColor: ['#1DCD9F', '#FFD63A', '#FF6363'],
          borderWidth: 0
        }]
      },
      options: {
        cutout: '70%',
        plugins: {
          legend: { display: false }
        }
      }
    });
  }

  getPriorityClass(priority: number): string {
    switch (priority) {
      case 0: return 'low';
      case 1: return 'medium';
      case 2: return 'high';
      case 3: return 'critical';
      default: return 'unknown';
    }
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

  getStatusClass(status: number): string {
    const map: { [key: number]: string } = {
      0: 'not-started',
      1: 'in-progress',
      2: 'blocked',
      3: 'on-hold',
      4: 'cancelled',
      5: 'completed'
    };
    return map[status] || 'unknown';
  }

  getStatusLabel(status: number): string {
    const map: { [key: number]: string } = {
      0: 'Not Started',
      1: 'In Progress',
      2: 'Blocked',
      3: 'On Hold',
      4: 'Cancelled',
      5: 'Completed'
    };
    return map[status] || 'Unknown';
  }

  gotofiltertask(statusLabel: string) {
    this.router.navigate(['/mytasks'], {
      queryParams: { status: statusLabel }
    });
  }
}
