import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {

  private themeKey = 'theme';

  constructor() {
    this.loadInitialTheme();
  }

  toggleTheme(): void {
    const body = document.getElementsByTagName('body')[0];
    body.classList.toggle('dark-theme');

    const isDark = body.classList.contains('dark-theme');
    localStorage.setItem(this.themeKey, isDark ? 'dark' : 'light');
  }

  loadInitialTheme(): void {
    const savedTheme = localStorage.getItem(this.themeKey);
    if (savedTheme === 'dark') {
      document.getElementsByTagName('body')[0].classList.add('dark-theme');
    }
  }
}
