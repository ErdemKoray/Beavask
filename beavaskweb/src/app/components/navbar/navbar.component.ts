import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  constructor(private routerlink: Router) {

  }

  gotohome(){
    this.routerlink.navigate(['/']);
  }

  gotoaccount(){
    this.routerlink.navigate(['/login']);
  }
}
