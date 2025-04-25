import { AfterViewInit, Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastService } from '../../../components/toast/toast.service';
import { CommonModule } from '@angular/common';
import { SortPipe } from '../../../common/pipe/sort.pipe';


@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, SortPipe], 
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.css'
})
export class ProjectsComponent implements AfterViewInit {
  constructor(
    private router: Router,
    private toast: ToastService
  ) {}

 
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

  searchForm = new FormGroup({
    searchname: new FormControl('', Validators.required)
  });

   
   private originalItems = [...this.items];


  searchProject(): void {
    if (this.searchForm.valid) {
      const searchValue = this.searchForm.value.searchname?.toLowerCase() ?? '';

     
      const result = this.originalItems.filter(item =>
        item.subject.toLowerCase().includes(searchValue) ||
        item.requestedBy.name.toLowerCase().includes(searchValue)
      );

      console.log('Arama sonucu:', result);

      if (result.length === 0) {
         
         this.items = [];
        this.toast.show({
          title: 'Sonuç bulunamadı!',
          message: `“${searchValue}” ile eşleşen bir kayıt bulunamadı.`
        });
      } else {
        
         this.items = result;
        this.toast.show({
          title: 'Başarılı!',
          message: `${result.length} kayıt bulundu.`
        });
      
      }
   
    } else {
         this.items = [...this.originalItems]; 
         this.toast.show({
             title: 'Arama Temizlendi',
             message: 'Tüm kayıtlar gösteriliyor.'
         });
    }
  }

   onSearchInputChange(event: any): void {
    if (!event.target.value) {
      this.items = [...this.originalItems];
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