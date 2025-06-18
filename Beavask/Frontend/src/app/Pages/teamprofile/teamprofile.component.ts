import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, RouterLink } from "@angular/router";
import { ReactiveFormsModule } from "@angular/forms";

import { TeamService } from "../../common/services/team/team.service";
import { CompanyService } from "../../common/services/company/company.service";
import { ProjectsService } from "../../common/services/projects/projects.service";
import { TaskService } from "../../common/services/task/task.service";
import { AuthprofileService } from "../../common/services/profile/authprofile.service";

import { Team } from "../../common/model/team.model";
import { companyTeam } from "../../common/services/team/model/company-team.model";
import { CompanyProject } from "../../common/services/company/model/companyProject.model";
import { Task } from "../../common/services/task/taskModel/task.model";
import { UserService } from "../../common/services/user.service";
import { teamMember } from "../../common/services/team/model/teamMember.model";

@Component({
  selector: 'app-teamprofile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,RouterLink],
  templateUrl: './teamprofile.component.html',
  styleUrl: './teamprofile.component.css'
})
export class TeamprofileComponent implements OnInit {

  teamId: number = 0;
  team: Team | null = null;
  headerImagePath = 'https://placehold.co/1200x300?text=Team+Banner';
  isLoading = true;
  errorMessage: string | null = null;

 members: teamMember[] = [];


  projects: CompanyProject[] = [];
  tasksMap: { [projectId: number]: Task[] } = {};

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private teamService: TeamService,
    private companyService: CompanyService,
    private projectsService: ProjectsService,
    private taskService: TaskService,
    private authService: AuthprofileService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    // URL parametreden takım id alınır
    this.route.params.subscribe(params => {
      this.teamId = +params['id'];
      if (!this.teamId) {
        this.router.navigate(['/']);
        return;
      }
      this.loadTeamProfile();
    });
  }

  loadTeamProfile(): void {
    this.isLoading = true;

    // Takım detayını getir
    this.teamService.getById(this.teamId).subscribe({
      next: res => {
        this.team = res.data;
        this.isLoading = false;
      },
      error: () => this.handleError()
    });

    // Takım üyeleri
    this.teamService.getTeamMember(this.teamId).subscribe({
      next: res => {
        this.members = res.data.teamMembers || [];
      },
      error: () => this.handleError()
    });

    // Kimlik bilgilerini al ve devam et
    this.authService.whoami().subscribe({
      next: user => {
        const userId = Number(user.userId);
        console.log('Kullanıcı ID:', userId);
        // Kullanıcıdan companyId alınır
this.userService.getUserByIdn(userId).subscribe({
  next: response => {
    const userInfo = response.data;
    console.log('Kullanıcı Bilgileri:', userInfo);
    if (!userInfo.companyId) {
      console.warn('Kullanıcının şirket bilgisi bulunamadı.');
      return;
    }
    
    const companyId = userInfo.companyId;
      

    // Şirket projelerini al (son 2-3 tanesi)
    this.companyService.getCompanyProjects().subscribe({

      next: projects => {
        console.log('Şirket projeleri:', projects);
        this.projects = projects.slice(-3);

        // Her proje için görevleri al, max 5 tanesi
        this.projects.forEach(proj => {
          this.taskService.getAllTasks(proj.id).subscribe({
            next: tasksRes => {
              this.tasksMap[proj.id] = tasksRes.data.slice(0, 5);
            }
          });
        });
      }
    });
  },
  error: err => {
    console.error('Kullanıcı bilgisi alınamadı', err);
  }
});

      },
      error: err => {
        console.error('Kullanıcı bilgisi alınamadı', err);
      }
    });
  }

  handleError(): void {
    this.isLoading = false;
    this.errorMessage = 'Team not found! Redirecting...';
    setTimeout(() => this.router.navigate(['/']), 2000);
  }

  // Görev ve proje linkleri
  getProjectLink(projectId: number): string {
    return `/project-detail/board/${projectId}?projectId=${projectId}`;
  }

  getTaskLink(projectId: number, taskId: number): string {
    return `/project-detail/board/${projectId}?TaskId=${taskId}`;
  }


}
