import { AfterViewInit, Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastService } from '../../../components/toast/toast.service';
import { CommonModule, DatePipe } from '@angular/common';
import { SortPipe } from '../../../common/pipe/sort.pipe';
import { GithubrepoService } from '../../../common/services/projects/githubrepo.service';
import { GithubRepo } from '../../../common/model/githubrepo.model';


@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, SortPipe,DatePipe], 
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.css'
})
export class ProjectsComponent implements AfterViewInit {
  constructor(
    private router: Router,
    private toast: ToastService,
    private githubRepoService: GithubrepoService,
  ) {}

  githubrepo : GithubRepo[] = [];
  getProjectRepos() {
    this.githubRepoService.getGithubRepos().subscribe({
      next:(response) => {
        console.log(response.data);
       this.toast.show({
          title: 'Success!',
          message: `Fetched ${response.data.length} repositories from Github.`
        });
        this.githubrepo = response.data
        console.log(this.githubrepo);
      },
      error:(err) => {
        this.toast.show({
          title: 'Error',
          message: 'Failed to fetch Github repositories.'
        });
      }
    });
    
  }

 
  sortKey: string = 'id'; 
  sortDirection: 'asc' | 'desc' = 'asc'; 

   items = [
    {
      id: 9852,
      requestedBy: {
        name: 'Debra J. Wilson',
        avatar: 'assets/images/debra.jpg'
      },
      subject: 'Your item has been updated!',
      assignee: {
        avatar: 'assets/images/user1.jpg'
      },
      priority: 'High',
      status: 'Open',
      createdDate: '01/04/2017',
      dueDate: '21/05/2017'
    },
    {
      id: 9501,
      requestedBy: {
        name: 'Amy R. Barnaby',
        avatar: 'assets/images/amy.jpg'
      },
      subject: 'Homeworth for your property increased',
      assignee: {
        avatar: 'assets/images/user2.jpg'
      },
      priority: 'Low',
      status: 'Open',
      createdDate: '01/04/2017',
      dueDate: '21/05/2017'
    },
    {
       id: 9600,
       requestedBy: {
         name: 'Charles K. Davis',
         avatar: 'assets/images/charles.jpg'
       },
       subject: 'Project proposal review',
       assignee: {
         avatar: 'assets/images/user3.jpg'
       },
       priority: 'Medium',
       status: 'Closed',
       createdDate: '15/03/2017',
       dueDate: '30/04/2017'
     },
     {
       id: 9123,
       requestedBy: {
         name: 'Debra J. Wilson',
         avatar: 'assets/images/debra.jpg'
       },
       subject: 'Meeting minutes required',
       assignee: {
         avatar: 'assets/images/user1.jpg'
       },
       priority: 'High',
       status: 'Pending',
       createdDate: '10/04/2017',
       dueDate: '12/04/2017'
     }
   
  ];
  searchProject(): void {
    const searchValue = this.searchForm.value.searchname?.toLowerCase() ?? '';

    const result = this.originalItems.filter(item =>
      item.subject.toLowerCase().includes(searchValue) ||
      item.requestedBy.name.toLowerCase().includes(searchValue)
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

  // Github projelerini aramak için
  searchpProject(): void {
    const searchValue = this.searchPForm.value.searchpname?.toLowerCase() ?? '';

    const result = this.originalpItems.filter(item =>
      item.name.toLowerCase().includes(searchValue)
    );

    if (result.length === 0) {
      this.githubrepo = [];
      this.toast.show({
        title: 'No Results',
        message: `No repositories found for "${searchValue}".`
      });
    } else {
      this.githubrepo = result;
      this.toast.show({
        title: 'Success!',
        message: `${result.length} repositories found.`
      });
    }
  }

 

  
  private originalItems: any[] = [...this.items];
  private originalpItems: any[] = [...this.githubrepo];


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

  onSearchpInputChange(event: any): void {
    if (!event.target.value) {
      this.githubrepo = [...this.originalpItems]; // Arama sıfırlandığında orijinal Github verisini geri yükle
    }
  }

  goToDetail(projectId?: number): void {
    const path = projectId ? `/project-detail/${projectId}` : '/project-detail/board';
    this.router.navigate([path]);
  }

  ngAfterViewInit(): void {
    const tooltipTriggerList = Array.from(
      document.querySelectorAll('[data-bs-toggle="tooltip"]')
    );
    tooltipTriggerList.forEach((tooltipTriggerEl: any) => {
      new (window as any).bootstrap.Tooltip(tooltipTriggerEl);
    });

    this.getProjectRepos();
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