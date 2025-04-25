import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ThemeService } from './common/services/theme/theme.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports:[RouterOutlet],
  template: ` <router-outlet></router-outlet> `,    
})
export class AppComponent implements OnInit {
  title = 'beavaskweb';

  constructor(private themeService: ThemeService) {}

  ngOnInit(): void {
    this.themeService.loadInitialTheme();
  }
}
