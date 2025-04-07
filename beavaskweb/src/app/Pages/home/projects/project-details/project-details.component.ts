import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PnavbarComponent } from './project-detail/pnavbar/pnavbar.component';

@Component({
  selector: 'app-project-details',
  standalone: true,
  imports: [RouterOutlet,PnavbarComponent],
  templateUrl: './project-details.component.html',
  styleUrl: './project-details.component.css'
})
export class ProjectDetailsComponent {

}
