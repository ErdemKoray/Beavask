import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../common/services/auth/auth.service';
import { auth } from '../../../common/model/auth.model';
import { CommonModule } from '@angular/common';
import { ToastData, ToastService } from '../../../components/toast/toast.service';
import { ToastComponent } from '../../../components/toast/toast.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,ToastComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{
  project: auth | null = null;
  IsLoading : boolean = false;
  darkMode = false;
  constructor(private router:Router,private authservice:AuthService,private toast:ToastService) {}


  ngOnInit(): void {
    localStorage.getItem('theme')=== 'dark' ? this.darkMode = true : this.darkMode = false;
  }
  gotohome(){
    this.router.navigate(['/']);
  }

  gotoregister() {
    this.router.navigate(['register']);
  }

  login(){
    
  }

  getbyId(){
   
  }
}
