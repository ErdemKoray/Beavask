import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, AbstractControl, ValidationErrors, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../../common/services/auth/auth.service';
import { ToastService } from '../../../components/toast/toast.service';

// Şifre ve doğrulama için custom validator
export function passwordMatchValidator(group: AbstractControl): ValidationErrors | null {
  const password = group.get('password')?.value;
  const confirmPassword = group.get('confirmPassword')?.value;
  return password === confirmPassword ? null : { passwordsNotMatching: true };
}

@Component({
  selector: 'app-forget-password',
  standalone: true,
  imports: [FormsModule, CommonModule, TranslateModule, ReactiveFormsModule],
  templateUrl: './forget-password.component.html',
  styleUrl: './forget-password.component.css'
})
export class ForgetPasswordComponent implements OnInit {

  @Output() openModal = new EventEmitter<boolean>();

  sendMail!: FormGroup;
  changePassword!: FormGroup;

  forgetPasswordMail = '';
  MailSendToUser = false;
  verifiedMail = false;
codeInput = ''
  constructor(private authService: AuthService, private fb: FormBuilder,private toast:ToastService) {}

  ngOnInit(): void {
    this.sendMail = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });

    this.changePassword = this.fb.group({
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+=-]).{8,}$')
        ]
      ],
      confirmPassword: ['', Validators.required]
    }, { validators: passwordMatchValidator });
  }

  toogleForgotPassModal() {
    this.openModal.emit(false);
    this.forgetPasswordMail = '';
  this.MailSendToUser = false;
  this.verifiedMail = false;
  }

sendMailForPassword() {
  if (this.sendMail.valid) {
    const { email } = this.sendMail.value;
    console.log('Gönderilen email:', email);
    this.authService.sendMailToResetPassword(email).subscribe({
      next: res => {
        this.forgetPasswordMail = email;
        this.MailSendToUser = true;
      },
      error: err => {
        console.log('Mail gönderim hatası:', err);
      }
    });
  }
}


  verifyMailWithCode(code: string) {
    this.authService.VerifyMail(this.forgetPasswordMail, code).subscribe({
      next: res => {
        this.verifiedMail = true;
      }
    });
  }

changePasswordSubmit() {
  if (this.changePassword.invalid) {
    this.changePassword.markAllAsTouched();
    return;
  }

  const password = this.changePassword.value.password;
  const confirmPassword = this.changePassword.value.confirmPassword;

  this.authService.changePassword(this.forgetPasswordMail, password, confirmPassword).subscribe({
    next: (res) => {
      if (res.isSuccess) {
        this.toast.show({
          title: 'Success',
          message: 'Şifreniz başarıyla değiştirildi. Giriş yapabilirsiniz.',
        });
        this.toogleForgotPassModal();
      } else {
        this.toast.show({
          title: 'Hata',
          message: res.message || 'Şifre değiştirilemedi.',
        });
      }
    },
    error: (err) => {
      this.toast.show({
        title: 'Hata',
        message: err?.error?.message || 'Bir hata oluştu. Lütfen tekrar deneyin.',
      });
    }
  });
}
}
