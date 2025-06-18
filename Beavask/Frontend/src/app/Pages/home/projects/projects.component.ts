import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ToastService } from '../../../components/toast/toast.service';
import { CommonModule, DatePipe } from '@angular/common';
import { SortPipe } from '../../../common/pipe/sort.pipe';
import { GithubrepoService } from '../../../common/services/projects/githubrepo.service';
import { GithubRepo } from '../../../common/model/githubrepo.model';
import { Project } from '../../../common/model/project.model';
import { ProjectsService } from '../../../common/services/projects/projects.service';
import { TranslateModule } from '@ngx-translate/core';
import { InvitationService, pendingProjects } from '../../../common/services/invitation/invitation.service';


@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, SortPipe, DatePipe,RouterLink,TranslateModule], 
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.css'
})
export class ProjectsComponent implements OnInit {

  constructor(
    private router: Router,
    private toast: ToastService,
    private githubRepoService: GithubrepoService,
    private projectService: ProjectsService,
     private invitationService: InvitationService
  ) {}

  ngOnInit(): void {
    this.getMyProjects(); 
      this.getProjectInvitations()
  }
 
  sortKey: string = 'id'; 
  sortDirection: 'asc' | 'desc' = 'asc'; 


  items: Project[] = [];
  private originalItems: any[] = [...this.items];

  projectInvitations: pendingProjects[] = [];
  getMyProjects(): void {
    this.projectService.getAll().subscribe(response => {
      if (response?.data) {
        this.items = response.data;
        this.originalItems = [...this.items]; // Orijinal öğeleri kaydet
        console.log(this.items);
      }
    });
  }
  


  searchProject(): void {
    const searchValue = this.searchForm.value.searchname?.toLowerCase() ?? '';
  
    // Eğer arama kutusu boşsa, orijinal öğelere dön
    if (!searchValue) {
      this.items = [...this.originalItems]; // Arama sıfırlandığında orijinal öğeleri geri yükle
      return;
    }
  
    // Arama yapılan değerle eşleşen öğeleri filtrele
    const result = this.originalItems.filter(item =>
      item.name.toLowerCase().includes(searchValue) ||
      (item.description && item.description.toLowerCase().includes(searchValue)) // Description null veya undefined ise, geçerli arama yapılmasın
    );
  
    if (result.length === 0) {
      this.items = [];
      this.toast.show({
        title: 'No Results',
        message: `No projects found for "${searchValue}".`
      });
    } else {
      this.items = result;
      this.toast.show({
        title: 'Success!',
        message: `${result.length} records found.`
      });
    }
  }
  

 
getProjectInvitations() {
  this.invitationService.getPrpjectReq().subscribe({
    next: res => {
      // API response'u dizi mi? (genelde res.data bir array döner)
      if (Array.isArray(res.data)) {
        this.projectInvitations = res.data;
      } else {
        this.projectInvitations = [];
      }
    }
  });
}
  
acceptProject(invitationId: number) {
  this.invitationService.acceptProject(invitationId).subscribe({
    next: () => {
      this.projectInvitations = this.projectInvitations.filter(p => p.id !== invitationId);
      this.toast.show({ title: 'Başarılı', message: 'Davet kabul edildi!' });
      this.getMyProjects();
    }
  });
}
rejectProject(invitationId: number) {
  this.invitationService.rejectProject(invitationId).subscribe({
    next: () => {
      this.projectInvitations = this.projectInvitations.filter(p => p.id !== invitationId);
      this.toast.show({ title: 'Reddedildi', message: 'Davet reddedildi.' });
    }
  });
}



 searchForm = new FormGroup({
    searchname: new FormControl('', [Validators.required])
  });

  searchPForm = new FormGroup({
    searchpname: new FormControl('', [Validators.required])
  });
  // Arama kutusu inputu değiştiğinde
  onSearchInputChange(event: any): void {
    if (!event.target.value) {
      this.items = [...this.originalItems]; // Arama sıfırlandığında orijinal veriyi geri yükle
    }
  }



  goToDetail(projectId?: number): void {
    const path = projectId ? `/project-detail/${projectId}` : '/project-detail/board';
    this.router.navigate([path]);
  }



 
  setSort(key: string): void {
    if (this.sortKey === key) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortKey = key;
      this.sortDirection = 'asc';
    }
  
  }

 
  getSortIcon(key: string): string {
     if (this.sortKey !== key) {
       return ''; 
     }
    
     const ascIcon = `<svg xmlns="http://www.w3.org/2000/svg" height="24px" viewBox="0 -960 960 960" width="24px" fill="#e3e3e3"><path d="M320-440v-287L217-624l-57-56 200-200 200 200-57 56-103-103v287h-80ZM600-80 400-280l57-56 103 103v-287h80v287l103-103 57 56L600-80Z"/></svg>`; 
     const descIcon = `<svg xmlns="http://www.w3.org/2000/svg" height="24px" viewBox="0 -960 960 960" width="24px" fill="#e3e3e3"><path d="M640-520v287L743-336l57 56-200 200-200-200 57-56 103 103v-287h80ZM360-880l200 200-57 56-103-103v287h-80v-287l-103 103-57-56 200-200Z"/></svg>`; 

     return this.sortDirection === 'asc' ? ascIcon : descIcon;
  }

}