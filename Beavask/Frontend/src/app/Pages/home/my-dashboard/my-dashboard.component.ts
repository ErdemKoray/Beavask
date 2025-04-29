import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import {
  Chart,
  ChartConfiguration,
  ChartOptions,
  ChartType,
  registerables
} from 'chart.js';
Chart.register(...registerables);

@Component({
  selector: 'app-my-dashboard',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule],
  templateUrl: './my-dashboard.component.html',
  styleUrl: './my-dashboard.component.css'
})
export class MyDashboardComponent {
  constructor(private router: Router) {}
  recentTasks = [
    { id: 101, subject: 'Fix login bug', priority: 'High', status: 'Open', dueDate: '2025-04-27' },
    { id: 102, subject: 'Create task table component', priority: 'Medium', status: 'Pending', dueDate: '2025-04-30' },
    { id: 103, subject: 'Refactor dashboard layout', priority: 'Low', status: 'Closed', dueDate: '2025-04-23' },
    { id: 104, subject: 'Update API endpoints', priority: 'High', status: 'Open', dueDate: '2025-05-01' },
    { id: 105, subject: 'Design mobile version', priority: 'Medium', status: 'Pending', dueDate: '2025-05-03' }
  ];
  activeProjects = [
    { name: 'Dashboard Redesign', status: 'Open', owner: 'Koray Erdem' },
    { name: 'Bug Hunt Sprint', status: 'Pending', owner: 'Muhammed Emin Aldaş' },
    { name: 'API Refactor', status: 'Closed', owner: 'Ece Demir' }
  ];
  ngAfterViewInit(): void {
    this.createStatusChart();
  }

  createStatusChart(): void {
    const canvas = document.getElementById('statusChart') as HTMLCanvasElement;
    if (!canvas) return;

    const chart = new Chart(canvas, {
      type: 'doughnut',
      data: {
        labels: ['To Do', 'In Progress', 'Done'],
        datasets: [{
          data: [3, 2, 1],
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

  gotofiltertask(status: string) {
    console.log('Navigating to mytasks with status:');
    this.router.navigate(['/mytasks'], {
      queryParams: { status: status }
    });
  }
  
}
