import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [RouterLink,CommonModule],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.css'
})
export class NotFoundComponent implements OnInit {
  constructor() { }
  darkMode=false;
  private themeKey = 'theme';
  ngOnInit(): void {
      localStorage.getItem(this.themeKey)=== 'dark' ? this.darkMode = true : this.darkMode = false;
  }

}
