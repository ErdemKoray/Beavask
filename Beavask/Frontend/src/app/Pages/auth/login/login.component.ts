import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../common/services/auth/auth.service';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ToastService } from '../../../components/toast/toast.service';
import { CommonModule } from '@angular/common';
import { ToastComponent } from '../../../components/toast/toast.component';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ToastComponent, ReactiveFormsModule],
  templateUrl: './login.component.html',
  animations: [
    trigger('slideUp', [
      transition(':enter', [
        style({ transform: 'translateY(100%)' }),  // Başlangıçta eleman aşağıda
        animate('0.5s ease-out', style({ transform: 'translateY(0)' }))  // Yukarı doğru kayarak geliyor
      ]),
      transition(':leave', [
        animate('0.5s ease-in', style({ transform: 'translateY(100%)' }))  // Çıkarken aşağıya kayıyor
      ])
    ])
  ],
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  
  loginForm!: FormGroup;
  darkMode: boolean = false;
  isLoading: boolean = false;
  constructor(
    private router: Router,
    private authService: AuthService,
    private toastService: ToastService,
    private fb: FormBuilder
  ) {}

  showLogin = false;
  showRegister = true;

  ngOnInit(): void {
    this.showLogin = true;
    this.showRegister = false;
  
    this.darkMode = localStorage.getItem('theme') === 'dark';
    
    // Form ve validasyonlar
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],  
      password: ['', [Validators.required, Validators.minLength(6)]], 
    });
  }

  // GitHub Login yönlendirmesi
  loginWithGithub(): void {
    window.location.href = 'https://github.com/login/oauth/authorize?client_id=Ov23lioqh5NrJDct5not&scope=user:email&redirect_uri=http://localhost:4200/auth-callback';
  }

  // Google Login yönlendirmesi
  loginWithGoogle(): void {
    // Google login URL'si buraya eklenmeli
    console.log('Google login is not yet implemented.');
  }

  // Giriş yapma fonksiyonu
  login(): void {
    if (this.loginForm.invalid) {
      this.toastService.show({ title: 'Error', message: 'Please fill all fields correctly.' });
      return;
    }
  
    this.isLoading = true; // Yükleme işlemi başladığında
    const { email, password } = this.loginForm.value;
  
    // Giriş API çağrısı
    this.authService.login({ email, password }).subscribe({
      next: (res) => {
        this.isLoading = false; // Yükleme işlemi bittiğinde
        if (res.isSuccess) {
          this.toastService.show({ title: 'Success', message: 'Login Successful' });
          localStorage.setItem('jwtToken', res.data);  
          this.router.navigate(['/']);  
        } else {
          this.toastService.show({ title: 'Error', message: res.message });
        }
      },
      error: (err) => {
        this.isLoading = false; // Hata durumunda yükleme işlemi bitti
        // err.error kontrolü yaparak null veya undefined olma durumunu ele alıyoruz
        const errorMessage = err.error.message || 'An unexpected error occurred';
        this.toastService.show({ title: 'Error', message: errorMessage });
      }
    });
  }
  



  goToRegister() {
    this.showLogin = false;
    this.showRegister = true;
    setTimeout(() => {
      this.router.navigate(['/register'])
     }, 1000);
  }
}
