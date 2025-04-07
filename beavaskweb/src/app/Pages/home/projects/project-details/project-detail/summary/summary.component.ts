import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-summary',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './summary.component.html',
  styleUrl: './summary.component.css'
})
export class SummaryComponent {
  types = [
    { name: 'Epik', percent: 40 },
    { name: 'Task', percent: 35 },
    { name: 'Alt Görev', percent: 25 }
  ];
  
}
