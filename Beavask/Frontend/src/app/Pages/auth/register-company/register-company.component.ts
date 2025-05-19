import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthCompanyService } from '../../../common/services/company/auth-company.service';
import { ToastService } from '../../../components/toast/toast.service';
import { CompanyRegisterModel } from '../../../common/services/company/model/createcompany.model';
import { ToastComponent } from '../../../components/toast/toast.component';
import { CommonModule } from '@angular/common';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-register-company',
  standalone: true,
  imports: [ToastComponent,ReactiveFormsModule,CommonModule],
  templateUrl: './register-company.component.html',
  styleUrl: './register-company.component.css'
})
export class RegisterCompanyComponent {
  companyForm!: FormGroup;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authCompanyService: AuthCompanyService,
    private toastService: ToastService,
    private route:Router
  ) {}

  ngOnInit(): void {
   this.companyForm = this.fb.group({
  name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
  description: ['', [Validators.maxLength(300)]],
  website: ['', [
    Validators.pattern(/^(https?:\/\/)?([\w\-]+\.)+[\w\-]{2,}([\/\w\-]*)*\/?$/)
  ]],
  email: ['', [Validators.required, Validators.email]],
  phoneNumber: ['', [
    Validators.required,
    Validators.pattern(/^(\+?\d{1,3}[- ]?)?\(?\d{3}\)?[- ]?\d{3}[- ]?\d{4}$/)
  ]],
  addressLine: ['', [Validators.required, Validators.minLength(5)]],
  city: ['', [Validators.required, Validators.pattern(/^[A-Za-zçğıöşüÇĞİÖŞÜ\s\-]+$/)]],
  country: ['', [Validators.required, Validators.pattern(/^[A-Za-zçğıöşüÇĞİÖŞÜ\s\-]+$/)]],
  postalCode: ['', [
    Validators.required,
    Validators.pattern(/^\d{4,10}$/)
  ]],
});

  }

 submit(): void {
  if (this.companyForm.invalid) {
    this.toastService.show({
      title: 'Error',
      message: 'Please fill in all required fields correctly.'
    });
    return;
  }

  this.isLoading = true;
  const model: CompanyRegisterModel = this.companyForm.value;

  this.authCompanyService.createCompany(model).subscribe({
    next: (message: string) => {
      this.toastService.show({
        title: 'Success',
        message: message
      });

      // ✅ email parametresiyle yönlendir
      this.route.navigate(['/verify'], {
        queryParams: { email: model.email }
      });

      this.companyForm.reset();
      this.isLoading = false;
    },
    error: err => {
      this.toastService.show({
        title: 'Error',
        message: err?.error || 'Unexpected error occurred.'
      });
      this.isLoading = false;
    }
  });
}


}
