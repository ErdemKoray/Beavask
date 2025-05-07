import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { PnavbarComponent } from './project-detail/pnavbar/pnavbar.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BoardComponent } from './project-detail/board/board.component';

@Component({
  selector: 'app-project-details',
  standalone: true,
  imports: [RouterOutlet,PnavbarComponent,CommonModule,FormsModule],
  templateUrl: './project-details.component.html',
  styleUrl: './project-details.component.css'
})
export class ProjectDetailsComponent   {

  constructor() { }
}
