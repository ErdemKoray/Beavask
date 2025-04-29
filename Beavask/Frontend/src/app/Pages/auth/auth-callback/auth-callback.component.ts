import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../common/services/auth/auth.service';
import { ToastService } from '../../../components/toast/toast.service';

@Component({
  selector: 'app-auth-callback',
  standalone: true,
  imports: [],
  templateUrl: './auth-callback.component.html',
  styleUrl: './auth-callback.component.css'
})
export class AuthCallbackComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService ,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const code = params['code'];
      if (code) {
       this.authService.githubLogin(code).subscribe({
          next: (response) => {
            localStorage.setItem('jwtToken', response.data); 
            this.toastService.show({ title: 'Success', message: 'Login successful!' });
            this.router.navigate(['/']); 
          },
          error: (err) => {
            this.toastService.show({ title: 'Error', message: 'Login failed!' });
            this.router.navigate(['/login']);
          }
        });
      }
    });
  }
  
}
