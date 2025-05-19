import { CommonModule } from '@angular/common';
import { AfterViewInit, Component } from '@angular/core';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-company-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './company-dashboard.component.html',
  styleUrl: './company-dashboard.component.css'
})
export class CompanyDashboardComponent  implements AfterViewInit {
  companyMetrics = [
    { count: 124, label: 'Total Tasks', subtext: 'Across teams' },
    { count: 38, label: 'Employees', subtext: 'Active this week' },
    { count: 12, label: 'Projects', subtext: 'Currently active' },
    { count: 6, label: 'Teams', subtext: 'In your company' },
  ];

  chartLegend = [
    { label: 'To Do', count: 60, colorClass: 'todo' },
    { label: 'In Progress', count: 45, colorClass: 'inprogress' },
    { label: 'Done', count: 130, colorClass: 'done' }
  ];

  activeEmployees = [
    { name: 'Emin Aldaş', team: 'Frontend', taskCount: 12, status: 'Active' },
    { name: 'Koray Erdem', team: 'Backend', taskCount: 9, status: 'Busy' },
    { name: 'Ece Demir', team: 'QA', taskCount: 6, status: 'Idle' }
  ];

  recentLogs = [
    { user: 'Emin Aldaş', action: 'created a task', module: 'Bug Fix', date: 'May 17' },
    { user: 'Koray Erdem', action: 'updated status', module: 'Dashboard Redesign', date: 'May 16' },
    { user: 'Ece Demir', action: 'commented', module: 'Sprint Review', date: 'May 15' },
  ];

  ngAfterViewInit(): void {
    this.renderCompanyChart();
  }

  renderCompanyChart(): void {
    const canvas = document.getElementById('companyTaskChart') as HTMLCanvasElement;
    if (!canvas) return;

    new Chart(canvas, {
      type: 'doughnut',
      data: {
        labels: this.chartLegend.map(i => i.label),
        datasets: [{
          data: this.chartLegend.map(i => i.count),
          backgroundColor: ['#22C55E', '#FACC15', '#3B82F6'],
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
}