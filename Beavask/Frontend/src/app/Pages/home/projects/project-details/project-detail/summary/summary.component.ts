import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import {
  Chart,
  ChartConfiguration,
  ChartOptions,
  ChartType,
  registerables
} from 'chart.js';
import { TaskService } from '../../../../../../common/services/task/task.service';
import { Task } from '../../../../../../common/services/task/taskModel/task.model';
import { TranslateModule } from '@ngx-translate/core';
Chart.register(...registerables);

@Component({
  selector: 'app-summary',
  standalone: true,
  imports: [CommonModule,TranslateModule],
  templateUrl: './summary.component.html',
  styleUrl: './summary.component.css'
})
export class SummaryComponent implements AfterViewInit,OnInit{

  @Input() projectId:number=0;
  ngAfterViewInit(): void {
    this.createStatusChart();
    this.createPriorityChart();
  }
    column: { [key: string]: Task[] } = {
      NotStarted: [],
      InProgress: [],
      OnHold: [],
      Cancelled: [],
      Completed: []
    };
    statusType=[
      {name:'Completed',count:0},
      {name:'Inprogress',count:0},
      {name:'Created',count:0},
      {name:'Canceled',count:0}
    ]
  types = [
    { name: 'Epik', percent: 40 },
    { name: 'Task', percent: 35 },
    { name: 'Alt Görev', percent: 25 }
  ];
  
  
  constructor(private taskService:TaskService){

  }
  
  ngOnInit(): void {
     this.filterTaskByStatus();
  }

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
  

  filterTaskByStatus(){
    this.taskService.getAllTasks(this.projectId).subscribe({
      next: (Response)=>{
        this.groupTasksByStatus(Response.data)
      },
      error:(err)=>{

      }
    })
  }

  groupTasksByStatus(tasks:Task[]) {
    console.log(tasks)
    Object.keys(this.column).forEach(status => {
      this.column[status] = [];
    });
    tasks.forEach(task => {

      switch (task.status) {
        case 0:
          this.column['NotStarted'].push(task);
          this.statusType[2].count =this.statusType[2].count +1  
          break;
        case 1:
          this.column['InProgress'].push(task);
          this.statusType[1].count =this.statusType[1].count +1  
          break;
        case 4:
          this.column['Cancelled'].push(task);
          this.statusType[3].count = this.statusType[3].count +1 

          break;
        case 5:
          this.column['Completed'].push(task);
          this.statusType[0].count = this.statusType[0].count +1; 
          break;
        default:
          break;
      }
    });
    console.log(this.statusType)
  }
}
