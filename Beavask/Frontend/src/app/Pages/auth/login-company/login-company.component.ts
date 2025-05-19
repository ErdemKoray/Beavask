import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthCompanyService } from '../../../common/services/company/auth-company.service';
import { ToastService } from '../../../components/toast/toast.service';
import { Router } from '@angular/router';
import { ToastComponent } from '../../../components/toast/toast.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login-company',
  standalone: true,
  imports: [ToastComponent,ReactiveFormsModule,CommonModule],
  templateUrl: './login-company.component.html',
  styleUrl: './login-company.component.css'
})
export class LoginCompanyComponent implements OnInit {
  loginForm!: FormGroup;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthCompanyService,
    private toast: ToastService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  login(): void {
    if (this.loginForm.invalid) {
      this.toast.show({ title: 'Error', message: 'Please fill all fields correctly.' });
      return;
    }

    this.isLoading = true;

    this.authService.login(this.loginForm.value).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.isSuccess) {
          localStorage.setItem('jwtToken', res.data);
          this.toast.show({ title: 'Success', message: 'Login successful.' });
          this.router.navigate(['/company/profile']); // yönlendirme
        } else {
          this.toast.show({ title: 'Error', message: res.message });
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.toast.show({
          title: 'Error',
          message: err?.error?.message || 'Login failed.'
        });
      }
    });
  }
}
