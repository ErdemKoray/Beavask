import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastComponent } from "../../../components/toast/toast.component";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ToastComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  constructor(private router:Router) {}
  gotologin() {
    this.router.navigate(['login']);
  }

  gotohome(){
    this.router.navigate(['/']);
  }
}
