import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent {

  
    recentProjects = [
      {
        name: 'Beavask',
        openIssues: 3,
        doneIssues: 5
      },
      {
        name: 'Kripto Ödeme Ağı',
        openIssues: 1,
        doneIssues: 2
      }
    ];
  
    activities = [
      {
        title: 'ldkşsgsş',
        code: 'BEAV-8',
        project: 'beavask',
        action: 'Created',
        userInitials: 'MA'
      },
      {
        title: 'd',
        code: 'BEAV-7',
        project: 'beavask',
        action: 'Created',
        userInitials: 'MA'
      },
      {
        title: 'cljkhl',
        code: 'BEAV-6',
        project: 'beavask',
        action: 'Updated',
        userInitials: 'MA'
      },
      {
        title: 'xcfd',
        code: 'BEAV-5',
        project: 'beavask',
        action: 'Created',
        userInitials: 'MA'
      }
    ];
  
 
  
}
