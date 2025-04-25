import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  Chart,
  ChartConfiguration,
  ChartOptions,
  ChartType,
  registerables
} from 'chart.js';
Chart.register(...registerables);

@Component({
  selector: 'app-summary',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './summary.component.html',
  styleUrl: './summary.component.css'
})
export class SummaryComponent {

  ngAfterViewInit(): void {
    this.createStatusChart();
    this.createPriorityChart();
  }
  
  types = [
    { name: 'Epik', percent: 40 },
    { name: 'Task', percent: 35 },
    { name: 'Alt Görev', percent: 25 }
  ];
  

  createStatusChart(): void {
    const statusCtx = document.getElementById('statusChart') as HTMLCanvasElement;
  
    new Chart(statusCtx, {
      type: 'doughnut',
      data: {
        labels: ['To Do', 'In Progress', 'Done'],
        datasets: [{
          data: [3, 2, 1], 
          backgroundColor: [
            '#1DCD9F', 
            '#FFD63A', 
            '#FF6363'  
          ],
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
  createPriorityChart(): void {
    const ctx = document.getElementById('priorityChart') as HTMLCanvasElement;
  
    if (!ctx) return;
  
    new Chart(ctx, {
      type: 'bar',
      data: {
        labels: ['Low', 'Medium', 'High'],
        datasets: [{
          label: 'Tasks by Priority',
          data: [4, 7, 2],
          backgroundColor: ['#95a5a6', '#f39c12', '#e74c3c'],
          borderRadius: 6,
          borderSkipped: false,
          barPercentage: 0.5,
          categoryPercentage: 0.7
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: {
            display: false
          }
        },
        scales: {
          x: {
            grid: {
              display: false
            },
            ticks: {
              color: getComputedStyle(document.documentElement).getPropertyValue('--card-sblue-white-color').trim(),
              font: {
                size: 12,
                weight: 500  
              }
            }
          },
          y: {
            beginAtZero: true,
            grid: {
              display: false
            },
            ticks: {
              color: getComputedStyle(document.documentElement).getPropertyValue('--card-sblue-white-color').trim(),
              font: {
                size: 12,
                weight: 500
              }
            }
          }
        }
      }
    });
  }
  
}
