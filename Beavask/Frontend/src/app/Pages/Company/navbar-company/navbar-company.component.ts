import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CompanyProfileService } from '../../../common/services/company/profile-company/company-profile.service';
import { CompanyProfile } from '../../../common/services/company/profile-company/model/companyProfile.model';
import { ToastService } from '../../../components/toast/toast.service';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-navbar-company',
  standalone: true,
  imports: [CommonModule, TranslateModule,RouterLink],
  templateUrl: './navbar-company.component.html',
  styleUrl: './navbar-company.component.css'
})
export class NavbarCompanyComponent implements OnInit {
   companyInfo: CompanyProfile | null = null;
  companyLogoUrl: string = 'default-logo.png'; // Fallback görsel
  darkMode = false;

  constructor(
    private companyService: CompanyProfileService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getCompanyInfo();
    this.darkMode = localStorage.getItem('theme') === 'dark';
  }

  getCompanyInfo(): void {
    this.companyService.whoamiCompany().subscribe({
      next: res => {
        this.companyInfo = res;
        this.companyLogoUrl = res.companyLogoUrl || this.generateInitialsLogo(res.companyName);
      },
      error: () => {
        this.companyInfo = null;
      }
    });
  }

  generateInitialsLogo(name: string): string {
    const canvas = document.createElement('canvas');
    canvas.width = 100;
    canvas.height = 100;
    const ctx = canvas.getContext('2d');
    if (!ctx) return '';
    ctx.fillStyle = '#2c3e50';
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    ctx.fillStyle = '#ecf0f1';
    ctx.font = '40px sans-serif';
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';
    const initials = name.split(' ').map(w => w[0]).join('').substring(0, 2).toUpperCase();
    ctx.fillText(initials, 50, 50);
    return canvas.toDataURL();
  }



  logout(): void {
    localStorage.removeItem('jwtToken');
    this.router.navigate(['/lcompany']);
  }
  goToDashboard() {
  this.router.navigate(['/company']);
}
}
