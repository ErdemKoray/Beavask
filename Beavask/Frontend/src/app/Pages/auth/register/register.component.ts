import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastService } from '../../../components/toast/toast.service';
import { AuthService } from '../../../common/services/auth/auth.service';
import { ToastComponent } from '../../../components/toast/toast.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ToastComponent, CommonModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm!: FormGroup;
  darkMode = false;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Dark mode kontrolü
    this.darkMode = localStorage.getItem('theme') === 'dark';

    // Form oluşturma ve validasyonlar
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordsMatchValidator });
  }

  // Şifre eşleşme doğrulayıcı
  private passwordsMatchValidator(group: AbstractControl) {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordsNotMatching: true };
  }

  // GitHub Login yönlendirmesi
  loginWithGithub() {
    window.location.href = 'https://github.com/login/oauth/authorize?client_id=Ov23lioqh5NrJDct5not&scope=user:email&redirect_uri=http://localhost:4200/auth-callback';
  }

  // Kullanıcı kaydını gerçekleştir
  register(): void {
    if (this.registerForm.invalid) {
      this.toastService.show({ title: 'Error', message: 'Please fill all required fields correctly.' });
      return;
    }

    this.isLoading = true;

    const formData = this.registerForm.value;
    const registerPayload = {
      firstName: formData.firstName,
      lastName: formData.lastName,
      email: formData.email,
      password: formData.password
    };

    this.authService.register(registerPayload).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response.isSuccess) {
          this.toastService.show({ title: 'Success', message: 'Registration successful!' });
          this.router.navigate(['/']);
        } else {
          this.toastService.show({ title: 'Error', message: response.message });
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.toastService.show({ title: 'Error', message: 'An error occurred during registration.' });
      }
    });
  }

  // Giriş sayfasına yönlendirme
  gotologin(): void {
    this.router.navigate(['/login']);
  }
}
