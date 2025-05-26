import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastService } from '../../../components/toast/toast.service';
import { AuthService } from '../../../common/services/auth/auth.service';
import { ToastComponent } from '../../../components/toast/toast.component';
import { CommonModule } from '@angular/common';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ToastComponent, CommonModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  animations: [
    trigger('slideDown', [
      transition(':enter', [
        style({ transform: 'translateY(-100%)' }),
        animate('0.5s ease-out', style({ transform: 'translateY(0)' }))
      ]),
      transition(':leave', [
        animate('0.5s ease-in', style({ transform: 'translateY(-100%)' }))
      ])
    ])
  ],
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm!: FormGroup;
  darkMode = false;
  isLoading = false;
  showLogin = false;
  showRegister = true;
  companyId: string | null = null;  // companyId saklamak için

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router,
    private route: ActivatedRoute  // eklendi
  ) {}

  ngOnInit(): void {
    this.showLogin = false;
    this.showRegister = true;

    this.darkMode = localStorage.getItem('theme') === 'dark';

    // Query parametreden companyId alınır
    this.route.queryParams.subscribe(params => {
      this.companyId = params['companyId'] || null;
    });
    console.log(this.companyId)

    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordsMatchValidator });
  }

  private passwordsMatchValidator(group: AbstractControl) {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordsNotMatching: true };
  }

  loginWithGithub() {
    // redirectUri şirket ID’sine göre değişiyor
    const baseRedirectUri = 'http://localhost:4200/auth-callback';
    const redirectUri = this.companyId 
      ? `http://localhost:4200/auth-callback?companyId=${this.companyId}` 
      : baseRedirectUri;

    const url = `https://github.com/login/oauth/authorize?client_id=Ov23lioqh5NrJDct5not&scope=user:email&redirect_uri=${encodeURIComponent(redirectUri)}`;

    window.location.href = url;
  }

  goToCompanyRegister(): void {
    this.router.navigate(['/rcompany']);
  }

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

  goToLogin() {
    this.showRegister = false;
    this.showLogin = true;
    setTimeout(() => {
      this.router.navigate(['/login']);
    }, 1000);
  }
}
