import { CommonModule, DatePipe } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core'; 
import { TeamService } from '../../../common/services/team/team.service';
import { Team } from '../../../common/model/team.model';
import { TranslateModule } from '@ngx-translate/core';
import { ProjectsService } from '../../../common/services/projects/projects.service';
import { Project } from '../../../common/model/project.model';
import { ToastService } from '../../../components/toast/toast.service';
import { Subscription } from 'rxjs';
import { RouterLink } from '@angular/router';


@Component({
  selector: 'app-main',
  standalone: true,
  imports: [CommonModule,TranslateModule,DatePipe,RouterLink],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})

export class MainComponent implements OnInit,OnDestroy  {



  private routeSub: Subscription = new Subscription();
  constructor(
    private teamService: TeamService,
     private projectService:ProjectsService,
    private toastService:ToastService
   
  ) {

  }
  

  recentProjects:Project[]=[];


  workedActivities = [
    { title: 'ldkşsgsş', code: 'BEAV-8', project: 'Beavask', action: 'Created', userInitials: 'MA' },
    { title: 'cljkhl', code: 'BEAV-6', project: 'Beavask', action: 'Updated', userInitials: 'MA' },
    { title: 'Yeni Özellik Geliştirme', code: 'BEAV-9', project: 'Beavask', action: 'Worked on', userInitials: 'MA' },
    { title: 'Dokümantasyon Güncelleme', code: 'BEAV-10', project: 'Beavask', action: 'Worked on', userInitials: 'MA' },
    { title: 'Performans İyileştirme', code: 'BEAV-11', project: 'Beavask', action: 'Worked on', userInitials: 'MA' },
    { title: 'Veritabanı Optimizasyonu', code: 'KRIP-4', project: 'Kripto Ödeme Ağı', action: 'Worked on', userInitials: 'MA' },
    { title: 'Güvenlik Açığı Tespiti', code: 'KRIP-5', project: 'Kripto Ödeme Ağı', action: 'Worked on', userInitials: 'MA' },
    { title: 'UI/UX Yeniden Tasarımı', code: 'BEAV-12', project: 'Beavask', action: 'Worked on', userInitials: 'MA' },
    { title: 'API Entegrasyonu', code: 'KRIP-6', project: 'Kripto Ödeme Ağı', action: 'Worked on', userInitials: 'MA' },
    { title: 'Test Senaryoları Yazımı', code: 'BEAV-13', project: 'Beavask', action: 'Worked on', userInitials: 'MA' },
     { title: 'Kod Refaktoring', code: 'BEAV-14', project: 'Beavask', action: 'Worked on', userInitials: 'MA' }, // Daha fazla veri eklendi
     { title: 'Yeni Modül Ekleme', code: 'KRIP-7', project: 'Kripto Ödeme Ağı', action: 'Worked on', userInitials: 'MA' },
  ];

  viewedActivities = [
    { title: 'xcfd', code: 'BEAV-5', project: 'Beavask', action: 'Viewed', userInitials: 'MA' },
    { title: 'Ana Sayfa İncelemesi', code: 'BEAV-1', project: 'Beavask', action: 'Viewed', userInitials: 'MA' },
    { title: 'Ödeme Akışı Analizi', code: 'KRIP-1', project: 'Kripto Ödeme Ağı', action: 'Viewed', userInitials: 'MA' },
     { title: 'Proje Genel Bakış', code: 'BEAV-2', project: 'Beavask', action: 'Viewed', userInitials: 'MA' }, // Daha fazla veri eklendi
  ];

  assignedActivities = [
    { title: 'Bug Fix: Kripto API', code: 'KRIP-3', project: 'Kripto Ödeme Ağı', action: 'Assigned', userInitials: 'MA' },
     { title: 'Task: Admin Paneli', code: 'BEAV-15', project: 'Beavask', action: 'Assigned', userInitials: 'MA' }, // Daha fazla veri eklendi
     { title: 'İyileştirme: Raporlama', code: 'KRIP-8', project: 'Kripto Ödeme Ağı', action: 'Assigned', userInitials: 'MA' },
  ];

  starredActivities = [
    { title: 'd', code: 'BEAV-7', project: 'Beavask', action: 'Starred', userInitials: 'MA' },
     { title: 'Önemli Özellik: Cüzdan', code: 'KRIP-9', project: 'Kripto Ödeme Ağı', action: 'Starred', userInitials: 'MA' }, // Daha fazla veri eklendi
  ];

  activeTab: string = 'worked';

  
  pageSize: number = 5; 
  currentPage: { [key: string]: number } = { 
    worked: 1,
    viewed: 1,
    assigned: 1,
    starred: 1
  };
  displayedActivities: any[] = []; 

user = {
  name: 'Mert Aydın',
  avatarUrl: '',

  avatar: '', 
};




  ngOnInit(): void {
   
    this.getUserProject();
    this.updateDisplayedActivities();
   

  }
  ngOnDestroy(): void {
    
  }
  setTab(tab: string): void {
    this.activeTab = tab;
    
    this.updateDisplayedActivities();
  }


  get activeActivitiesList(): any[] {
    switch (this.activeTab) {
      case 'worked':
        return this.workedActivities;
      case 'viewed':
        return this.viewedActivities;
      case 'assigned':
        return this.assignedActivities;
      case 'starred':
        return this.starredActivities;
      default:
        return [];
    }
  }

  
  get totalItems(): number {
    return this.activeActivitiesList.length;
  }


  get totalPages(): number {
    return Math.ceil(this.totalItems / this.pageSize);
  }

  
  get currentItemsRange(): string {
     if (this.totalItems === 0) return '0-0 / 0';
     const start = (this.currentPage[this.activeTab] - 1) * this.pageSize + 1;
     const end = Math.min(start + this.pageSize - 1, this.totalItems);
     return `${start}-${end} / ${this.totalItems}`;
  }


 
  get canGoPrev(): boolean {
    return this.currentPage[this.activeTab] > 1;
  }

  get canGoNext(): boolean {
    return this.currentPage[this.activeTab] < this.totalPages;
  }

  updateDisplayedActivities(): void {
    const currentPage = this.currentPage[this.activeTab];
    const startIndex = (currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;

    this.displayedActivities = this.activeActivitiesList.slice(startIndex, endIndex);
  }

  
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage[this.activeTab] = page;
      this.updateDisplayedActivities();
    }
  }

  nextPage(): void {
    this.goToPage(this.currentPage[this.activeTab] + 1);
  }

 
  prevPage(): void {
    this.goToPage(this.currentPage[this.activeTab] - 1);
  }


  get pageNumbers(): number[] {
    const pageCount = this.totalPages;
    
    if (pageCount <= 1) return [];

    return Array.from({ length: pageCount }, (_, i) => i + 1);
  }


  

getUserProject() {
  this.projectService.getAll().subscribe({
    next: (Response) => {
      this.recentProjects = Response.data
        .map((project: any) => ({
          id: project.id,
          name: project.name,
          description: '',
          createdAt: new Date(project.createdAt),
          isActive: true,
          customerId: 0
        }))
        .filter((project, index, self) => {
          return index >= self.length - 3;
        });
    },
    error: (err) => {
      const errorMessage = err?.error?.message || 'An unexpected error occurred';
      this.toastService.show({
        title: 'Error',
        message: 'An error occurred while fetching data: ' + errorMessage,
      });
    },
  });
}
}