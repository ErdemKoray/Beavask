import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastService } from '../../../../components/toast/toast.service';
import { AuthCompanyService } from '../../../../common/services/company/auth-company.service';
import { ToastComponent } from '../../../../components/toast/toast.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-valid-mail',
  standalone: true,
  imports: [ReactiveFormsModule,ToastComponent,CommonModule],
  templateUrl: './valid-mail.component.html',
  styleUrl: './valid-mail.component.css'
})
export class ValidMailComponent implements OnInit {
  email: string = '';
  codeForm!: FormGroup;
  isLoading = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private toast: ToastService,
    private authService: AuthCompanyService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'];
    });

    this.codeForm = new FormGroup({
      code1: new FormControl('', [Validators.required, Validators.pattern('[0-9]')]),
      code2: new FormControl('', [Validators.required, Validators.pattern('[0-9]')]),
      code3: new FormControl('', [Validators.required, Validators.pattern('[0-9]')]),
      code4: new FormControl('', [Validators.required, Validators.pattern('[0-9]')]),
      code5: new FormControl('', [Validators.required, Validators.pattern('[0-9]')]),
      code6: new FormControl('', [Validators.required, Validators.pattern('[0-9]')]),
    });
  }

verify(): void {
  if (this.codeForm.invalid || !this.email) {
    this.toast.show({ title: 'Error', message: 'Please fill all fields correctly.' });
    return;
  }

  const code = Object.values(this.codeForm.value).join('');
  this.isLoading = true;

  this.authService.verifyMail(this.email, code).subscribe({
    next: (res) => {
      this.isLoading = false;
      this.toast.show({ title: 'Success', message: res || 'Email verified successfully.' });
      this.router.navigate(['/lcompany']);
    },
    error: (err) => {
      this.isLoading = false;
      const errorMsg =
        err?.error?.message || err?.message || 'Verification failed.';
      this.toast.show({ title: 'Error', message: errorMsg });
    }
  });
}


  moveToNext(event: any, currentIndex: number): void {
    const inputLength = event.target.value.length;
    if (inputLength === 1 && currentIndex < 5) {
      const nextInput = document.getElementById('code' + (currentIndex + 2));
      if (nextInput) (nextInput as HTMLElement).focus();
    }
  }
}