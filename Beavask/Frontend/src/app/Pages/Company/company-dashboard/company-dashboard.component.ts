import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import {
  Chart,
  DoughnutController,
  ArcElement,
  Tooltip,
  Legend
} from 'chart.js';
import { CompanyService } from '../../../common/services/company/company.service';
import { AuthCompanyService } from '../../../common/services/company/auth-company.service';
import { ToastService } from '../../../components/toast/toast.service';
import { TeamService } from '../../../common/services/team/team.service';
import { map } from 'rxjs';
import { CompanyUser } from '../../../common/services/company/profile-company/company.model';

// Gerekli tüm bileşenleri register et
Chart.register(DoughnutController, ArcElement, Tooltip, Legend);

@Component({
  selector: 'app-company-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './company-dashboard.component.html',
  styleUrl: './company-dashboard.component.css'
})
export class CompanyDashboardComponent  implements AfterViewInit,OnInit {

  

  companyMetrics = [
    { count: 24, label: 'Total Tasks', subtext: 'Across teams' },
    { count: 0, label: 'Employees', subtext: 'Active this week' },
    { count: 0, label: 'Projects', subtext: 'Currently active' },
    { count: 0, label: 'Teams', subtext: 'In your company' },
  ];

  chartLegend = [
    { label: 'To Do', count: 60, colorClass: 'todo' },
    { label: 'In Progress', count: 45, colorClass: 'inprogress' },
    { label: 'Done', count: 130, colorClass: 'done' }
  ];

activeEmployees: CompanyUser[] = [];

  recentLogs = [
    { user: 'Emin Aldaş', action: 'created a task', module: 'Bug Fix', date: 'May 17' },
    { user: 'Koray Erdem', action: 'updated status', module: 'Dashboard Redesign', date: 'May 16' },
    { user: 'Ece Demir', action: 'commented', module: 'Sprint Review', date: 'May 15' },
  ];


  constructor(
    private companyService:CompanyService,
    private authCompanyService:AuthCompanyService,
    private toastService:ToastService,
    private teamService:TeamService
  ){

  }
  ngAfterViewInit(): void {
    this.renderCompanyChart();
  }

  ngOnInit(): void {
    this.countMembersCompany()
    this.countProjectsCompany()
     this.countTeamsCompany()
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





countMembersCompany() {
  this.authCompanyService.getWhoamiCompany().subscribe({
    next: (company) => {
      this.companyService.getCompanyAllUsers(Number(company.companyId)).subscribe({
        next: (users: CompanyUser[]) => {
          this.companyMetrics[1].count = users.length;

          this.activeEmployees = users.filter(user => user.isAssignedToCompany);
        },
        error: (er) => {
        
        }
      });
    },
    error: (err) => {
  
    }
  });
}


  countProjectsCompany(){
 
        this.companyService.getCompanyProjects().subscribe({
          next:(res)=>{
             this.companyMetrics[2].count=res.length
          },
          error:(er)=>{
          
          }
        });
      }

  countTeamsCompany(){


    this.teamService.getAllCompanyTeams().subscribe({
      next: (res) => {
      if (Array.isArray(res.data)) {
  this.companyMetrics[3].count = res.data.length;
} else if (res.data) {
  this.companyMetrics[3].count = 1; 
} else {
  this.companyMetrics[3].count = 0; 
}

      },
      error: (er) => {

    
  }
});
  }
}