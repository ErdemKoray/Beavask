import { Component } from '@angular/core';
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
export class LoginComponent {
  project: auth | null = null;
  IsLoading : boolean = false;

  constructor(private router:Router,private authservice:AuthService,private toast:ToastService) {}

  gotohome(){
    this.router.navigate(['/']);
  }

  gotoregister() {
    this.router.navigate(['register']);
  }

  login(){
    
  }

  getbyId(){
    this.authservice.getById(2).subscribe({
      next: (data) => {this.project = data
      this.IsLoading= true
      this.toast.show( {title:'başarı', message: 'An error occurred while fetching the data.'})
    },
      error: (err) => {this.toast.show(  {title:'Error', message: 'An error occurred while fetching the data.'});
      this.IsLoading = false;}
    });

    console.log(this.project);
  }
}
