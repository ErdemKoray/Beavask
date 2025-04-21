import { AfterViewInit, Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastService } from '../../../components/toast/toast.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.css'
})
export class ProjectsComponent implements AfterViewInit {
  constructor(private router: Router,private toast:ToastService) {}


  mockProjects = [
    {
      name: 'Beavask',
      owner: 'Muhammed Emin Aldaş',
      status: 'In Progress',
      updated: new Date('2025-04-11')
    },
    {
      name: 'UI Overhaul',
      owner: 'Koray Erdem',
      status: 'Completed',
      updated: new Date('2025-04-08')
    },
    {
      name: 'Sprint Review',
      owner: 'Kürdo',
      status: 'Planning',
      updated: new Date('2025-04-09')
    }
  ];

  
  searchForm = new FormGroup({
    searchname: new FormControl('', Validators.required)
  });

  searchProject(): void {
    if (this.searchForm.valid) {
      console.log(this.searchForm.value);
      this.searchForm.reset();
    }
  }

  goToDetail(): void {
    this.router.navigate(['/project-detail/board']);
  }

  ngAfterViewInit(): void {
    const tooltipTriggerList = Array.from(
      document.querySelectorAll('[data-bs-toggle="tooltip"]')
    );
    tooltipTriggerList.forEach((tooltipTriggerEl: any) => {
      new (window as any).bootstrap.Tooltip(tooltipTriggerEl);
    });

  }

  showToast() {
    this.toast.show({
      title: 'Başarılı!',
      message: 'İşlem başarıyla tamamlandı.'
   
    });
  }  
}