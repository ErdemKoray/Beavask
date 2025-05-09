import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { Task } from '../../../../../../../common/model/task.model';


@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [],
  templateUrl: './task-detail.component.html',
  styleUrl: './task-detail.component.css'
})
export class TaskDetailComponent implements AfterViewInit {

  @Input() taskD: Task | undefined; // taskD'yi undefined ile başlatıyoruz

  ngAfterViewInit() {
    console.log('TaskDetailComponent initialized');
    console.log(this.taskD); // Modal içine gelen görev detaylarını kontrol ediyoruz
  }
}
