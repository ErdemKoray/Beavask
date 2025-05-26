import { Component, OnInit } from '@angular/core';
import { companyTeam } from '../../../common/services/team/model/company-team.model';
import { TeamService } from '../../../common/services/team/team.service';
import { AuthCompanyService } from '../../../common/services/company/auth-company.service';
import { ToastService } from '../../../components/toast/toast.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiResponse } from '../../../common/model/apiResponse.model';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { CompanyService } from '../../../common/services/company/company.service';
import { CompanyUser } from '../../../common/services/company/profile-company/company.model';

@Component({
  selector: 'app-company-team',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './company-team.component.html',
  styleUrl: './company-team.component.css'
})
export class CompanyTeamComponent implements OnInit {
  teams: companyTeam[] = [];
  selectedTeam: companyTeam | null = null;
  isLoading = false;
  createTeamForm!: FormGroup;
  showAddMembers = false;
  availableUsers: any[] = []; // Üye ekleme için kullanılacak kullanıcılar

  constructor(
    private teamService: TeamService,
    private authService: AuthCompanyService,
    private toastService: ToastService,
    private fb: FormBuilder,
    private companyService:CompanyService
  ) {
    this.createTeamForm = this.fb.group({
      teamName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]]
    });
  }

  ngOnInit(): void {
    this.loadCompanyTeams();
  }

loadCompanyTeams(): void {
  this.isLoading = true;
  this.authService.getWhoamiCompany().subscribe({
    next: (user) => {
      this.teamService.getCompanyTeam(user.companyId).subscribe({
       next: (res: ApiResponse<companyTeam[] | companyTeam>) => {

  if (Array.isArray(res.data)) {
    this.teams = res.data;
  } else if (res.data) {
    this.teams = [res.data]; 
  } else {
    this.teams = [];
  }

  this.isLoading = false;
},
        error: () => {
          this.toastService.show({ title: 'Error', message: 'Failed to load teams.' });
          this.isLoading = false;
        }
      });
    },
    error: () => {
      this.toastService.show({ title: 'Error', message: 'User information could not be retrieved.' });
      this.isLoading = false;
    }
  });
}

selectTeam(team: companyTeam): void {
  this.selectedTeam = team;
  this.showAddMembers = false;
  this.loadTeamMembers(team.id);

  this.loadAvailableUsers();
}

loadTeamMembers(teamId: number): void {
  this.teamService.getTeamMember(teamId).subscribe({
    next: (res: ApiResponse<companyTeam>) => {
      if (this.selectedTeam && res.data) {
        this.selectedTeam.teamMembers = res.data.teamMembers || [];
        console.log('teamMembers:', this.selectedTeam.teamMembers);
      }
    },
    error: () => {
      this.toastService.show({ title: 'Error', message: 'Failed to load team members.' });
    }
  });
}

loadAvailableUsers(): void {
  this.authService.getWhoamiCompany().subscribe({
    next: user => {
      this.companyService.getCompanyAllUsers(user.companyId).subscribe({
        next: (users: CompanyUser[]) => {
          this.availableUsers = users || [];
        },
        error: () => {
          this.toastService.show({ title: 'Error', message: 'Failed to load users.' });
        }
      });
    },
    error: () => {
      this.toastService.show({ title: 'Error', message: 'User info could not be retrieved.' });
    }
  });
}


  toggleAddMembers(): void {
    this.showAddMembers = !this.showAddMembers;
  }

isMember(userId: number): boolean {
  if (!this.selectedTeam?.teamMembers || !Array.isArray(this.selectedTeam.teamMembers)) {
    return false;
  }
  return this.selectedTeam.teamMembers.some(m => m.id === userId);
}

  addMemberToTeam(teamId: number, userId: number): void {
    this.teamService.assignUserToTeam(teamId, userId).subscribe({
      next: () => {
        this.toastService.show({ title: 'Success', message: 'Member added to team.' });
        if (this.selectedTeam) {
          this.selectedTeam.teamMembers = this.selectedTeam.teamMembers || [];
          const addedUser = this.availableUsers.find(u => u.id === userId);
          if (addedUser) {
            this.selectedTeam.teamMembers.push(addedUser);
          }
        }
      },
      error: () => {
        this.toastService.show({ title: 'Error', message: 'Failed to add member.' });
      }
    });
  }
  deleteTeam(teamId: number): void {
    if (!confirm('Are you sure you want to delete this team?')) return;

    this.teamService.deleteTeamInfo(teamId).subscribe({
      next: () => {
        this.toastService.show({ title: 'Success', message: 'Team deleted successfully.' });
        this.teams = this.teams.filter(t => t.id !== teamId);
        if (this.selectedTeam?.id === teamId) this.selectedTeam = null;
      },
      error: () => {
        this.toastService.show({ title: 'Error', message: 'Failed to delete team.' });
      }
    });
  }

  assignMemberToTeam(){

  }
  createTeam(): void {
    if (this.createTeamForm.invalid) {
      this.toastService.show({ title: 'Warning', message: 'Please enter a valid team name (3-50 characters).' });
      return;
    }

    const name = this.createTeamForm.get('teamName')?.value.trim();

    this.authService.getWhoamiCompany().subscribe({
      next: user => {
        if (user && user.companyId) {
          this.isLoading = true;
          this.teamService.createTeam(name).subscribe({
            next: () => {
              this.toastService.show({ title: 'Success', message: 'Team created successfully.' });
              this.createTeamForm.reset();
              this.loadCompanyTeams();
              this.isLoading = false;
            },
            error: () => {
              this.toastService.show({ title: 'Error', message: 'Failed to create team.' });
              this.isLoading = false;
            }
          });
        } else {
          this.toastService.show({ title: 'Error', message: 'User not assigned to a company.' });
          this.isLoading = false;
        }
      },
      error: () => {
        this.toastService.show({ title: 'Error', message: 'User information could not be retrieved.' });
        this.isLoading = false;
      }
    });
  }
}
