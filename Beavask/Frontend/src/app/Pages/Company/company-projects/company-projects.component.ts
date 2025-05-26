import { Component, OnInit } from '@angular/core';
import { ProjectsService } from '../../../common/services/projects/projects.service';
import { Project } from '../../../common/model/project.model';
import { ToastService } from '../../../components/toast/toast.service';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { CompanyProfileService } from '../../../common/services/company/profile-company/company-profile.service';
import { Observable } from 'rxjs';
import { CreatprojectService } from '../../../common/services/projects/creatproject.service';
import { CompanyService } from '../../../common/services/company/company.service';
import { CompanyProject } from '../../../common/services/company/model/companyProject.model';
import { cProject } from '../../../common/services/projects/creatproject.model';

@Component({
  selector: 'app-company-projects',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DatePipe,RouterLink],
  templateUrl: './company-projects.component.html',
  styleUrls: ['./company-projects.component.css']
})
export class CompanyProjectsComponent implements OnInit {
projects: CompanyProject[] = [];
  showCreateProjectModal = false;
  createProjectForm!: FormGroup;
  isLoading = false;
  sortKey: keyof Project = 'createdAt';
  sortDirection: 'asc' | 'desc' = 'desc';
  companyId!: number;

  constructor(
    private projectsService: ProjectsService,
    private toastService: ToastService,
    private fb: FormBuilder,
    private router: Router,
    private companyProfileService: CompanyProfileService,
    private createProjectService:CreatprojectService,
    private companyService:CompanyService

  ) {
    this.createProjectForm = this.fb.group({
  repoUrl: ['', [Validators.required, Validators.pattern(/https?:\/\/github\.com\/.+\/.+/)]]
});

  }

  ngOnInit(): void {
    this.loadCompanyIdAndProjects();
  }

  private loadCompanyIdAndProjects(): void {
    this.companyProfileService.whoamiCompany().subscribe({
      next: (company) => {
        this.companyId = company.companyId;
        this.loadProjects();
      },
      error: () => {
        this.toastService.show({ title: 'Error', message: 'Failed to get company info.' });
      }
    });
  }
loadProjects(): void {
  if (!this.companyId) return;
  this.isLoading = true;
  this.companyService.getCompanyProjects().subscribe({
    next: (projects) => {
      console.log('Projects Array:', projects);
      this.projects = projects || [];
      this.sortProjects();
      this.isLoading = false;
    },
    error: () => {
      this.toastService.show({ title: 'Error', message: 'Failed to load projects.' });
      this.isLoading = false;
    }
  });
}


  sortProjects(): void {
    this.projects.sort((a, b) => {
    const aValue = (a as any)[this.sortKey];
const bValue = (b as any)[this.sortKey];


      if (aValue == null && bValue == null) return 0;
      if (aValue == null) return this.sortDirection === 'asc' ? -1 : 1;
      if (bValue == null) return this.sortDirection === 'asc' ? 1 : -1;

      if (aValue < bValue) return this.sortDirection === 'asc' ? -1 : 1;
      if (aValue > bValue) return this.sortDirection === 'asc' ? 1 : -1;
      return 0;
    });
  }

  setSort(key: keyof Project): void {
    if (this.sortKey === key) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortKey = key;
      this.sortDirection = 'asc';
    }
    this.sortProjects();
  }

  getSortIcon(key: keyof Project): string {
    if (this.sortKey !== key) return '';
    return this.sortDirection === 'asc' ? '&#9650;' : '&#9660;';
  }

  toggleCreateProjectModal(): void {
    this.showCreateProjectModal = !this.showCreateProjectModal;
  }

submitProject(): void {
  if (this.createProjectForm.invalid) {
    this.toastService.show({ title: 'Warning', message: 'Please enter the GitHub repository URL.' });
    return;
  }

  const projectData: cProject = {
    repoUrl: this.createProjectForm.value.repoUrl.trim()
    
  };
  console.log(projectData.repoUrl)

  this.createProjectService.create(projectData).subscribe({
    next: () => {
      this.toastService.show({ title: 'Success', message: 'Project created successfully.' });
      this.createProjectForm.reset();
      this.showCreateProjectModal = false;
      this.loadProjects();
    },
    error: (err) => {
      this.toastService.show({ title: 'Error', message: 'Failed to create project.' +err.message });
    }
  });
}

  goToProjectDetails(projectId: number): void {
    this.router.navigate(['/company/project-detail', projectId]);
  }
}
