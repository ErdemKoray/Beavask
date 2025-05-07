import { CommonModule } from '@angular/common';
import { AfterViewChecked, AfterViewInit, Component, HostListener, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InlineEditComponent } from './inline-edit/inline-edit.component';
import { TaskDetailComponent } from './task-detail/task-detail.component';
import { ShareModalComponent } from './share/share.component';
import { ProjectsService } from '../../../../../../common/services/projects/projects.service';
import { ActivatedRoute } from '@angular/router';

interface Task {
  id: number;
  title: string;
  description: string;
  status: string;
  priority: string;
  dueDate: Date;
  color: string;
  assignee: string;
  comments?: Comment[];
}
interface Column {
  id: number;
  title: string;
  tasks?: Task[];

}

@Component({
  selector: 'app-board',
  standalone: true,
  imports: [CommonModule,FormsModule,InlineEditComponent,TaskDetailComponent,ShareModalComponent],
  templateUrl: './board.component.html',
  styleUrl: './board.component.css'
})
export class BoardComponent implements AfterViewInit {

 projectId: number= 0;
  
  shareVisible = false;
  currentTaskUrl = 'https://example.com/task/CCS-3';
isCreateProjectOpen = false;
activeDetail=false

 
  constructor(private apiProject:ProjectsService,private route:ActivatedRoute) { }

  ngAfterViewInit() {
    this.getProjectDetail();
  }

  columns: Column[] = [
    { id: 1, title: 'To Do', tasks: [
      { id: 1, title: 'Task 1', description: 'Description 1', status: 'To Do', priority: 'High', dueDate: new Date(), color: 'var(--status-color-red)',assignee: 'User 1', },
      { id: 2, title: 'Task 2', description: 'Description 2', status: 'To Do', priority: 'Medium', dueDate: new Date(),color:'var(--status-color-red)', assignee: 'User 2',},
      { id: 5, title: 'Task 2', description: 'Description 2', status: 'To Do', priority: 'Medium', dueDate: new Date(),color:'var(--status-color-red)', assignee: 'User 2',},
      { id: 4, title: 'Task 2', description: 'Description 2', status: 'To Do', priority: 'Medium', dueDate: new Date(),color:'var(--status-color-red)', assignee: 'User 2',}
    ] },
    { id: 2, title: 'In Progress' 
        
        , tasks: [
          { id: 3, title: 'Task 3', description: 'Description 3', status: 'In Progress', priority: 'Low', dueDate: new Date(), color: 'var(--status-color-blue)', assignee: 'User 3' },
          { id: 4, title: 'Task 4', description: 'Description 4', status: 'In Progress', priority: 'High', dueDate: new Date(), color: 'var(--status-color-blue)', assignee: 'User 4' }
        ]
    },
    { id: 3, title: 'Done' ,tasks: [
      { id: 5, title: 'Task 5', description: 'Description 5', status: 'Done', priority: 'Medium', dueDate: new Date(),  color: 'var(--status-color-sdgreen)',assignee: 'User 5' },
      { id: 6, title: 'Task 6', description: 'Description 6', status: 'Done', priority: 'Low', dueDate: new Date(), color: 'var(--status-color-sdgreen)', assignee: 'User 6' }
    ] 
    }
  ];

  onTitleChange(col: Column, newTitle: string) {
    col.title = newTitle || 'Untitled';
   
  }

  addColumn() {
    const nextId = Math.max(...this.columns.map(c => c.id)) + 1;
    this.columns.push({ id: nextId, title: 'New Column' });
  }
  
  
  
  
  




//GENEL FONKSİYONLAR BÖLÜMÜ
showShare() { this.shareVisible = true; }
  toggleCreateTaskBoard() {
    this.isCreateProjectOpen = !this.isCreateProjectOpen;
    
  }
  
  toggleTaskDetail() {
    this.activeDetail = !this.activeDetail;
    
  }



  //API BÖLÜMÜ

  getProjectDetail() {
    this.route.params.subscribe(params => {
      this.projectId = +params['projectId']; 
      console.log(this.projectId)
    }
  );
    this.apiProject.getById(this.projectId).subscribe((response) => {
      if (response?.data) {
        console.log(response.data);
      }
    });
  }

  //API İLE İLGİLİ FONKSİYONLAR BÖLÜMÜ


  //HOST LISTENERS BÖLÜMÜ
  @HostListener('document:keydown.escape', ['$event'])
  onEscape(event: KeyboardEvent) {
    this.isCreateProjectOpen = false;
    this.activeDetail=false;
  }
  @HostListener('document:click', ['$event'])
  handleClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;
  

    if (
      this.isCreateProjectOpen &&
      !target.closest('.bv-cp-container') &&
      !target.closest('[data-dropdown="create"]')
    ) {
      this.isCreateProjectOpen = false;
    }

    if (
      this.activeDetail &&
      !target.closest('.task-modal') &&
      !target.closest('[data-modal="detail"]')
    ) {
      this.activeDetail = false;
    }
  }
  

}


