import { Component, OnInit } from '@angular/core';
import { CompanyService } from '../../../common/services/company/company.service';
import { AuthCompanyService } from '../../../common/services/company/auth-company.service';
import { Company } from '../../../common/model/company.model';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../../components/toast/toast.service';
import { CompanyProfileService } from '../../../common/services/company/profile-company/company-profile.service';
import {
  Chart,
  DoughnutController,
  ArcElement,
  Tooltip,
  Legend,
} from 'chart.js';

Chart.register(DoughnutController, ArcElement, Tooltip, Legend);

@Component({
  selector: 'app-cprofile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cprofile.component.html',
  styleUrl: './cprofile.component.css'
})
export class CprofileComponent implements OnInit {
  company: Company | null = null;
  deneme =true;
  isLoading = false;

  constructor(
    private authCompany: AuthCompanyService,
    private compprofile:CompanyProfileService,
    private companyService: CompanyService,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    this.loadCompanyProfile();
  }

  loadCompanyProfile(): void {
    this.isLoading = true;


    this.compprofile.whoamiCompany().subscribe({
      next: (res) => {
       const companyId = res.companyId;
if (!companyId) {
  this.toast.show({ title: 'Error', message: 'Company ID not found.' });
  this.isLoading = false;
  return;
}

        this.companyService.getCompanyById(companyId).subscribe({
          next: (companyRes) => {

          this.company = {
  ...companyRes.data,
  createdAt: new Date(companyRes.data.createdAt),
  updatedAt: new Date(companyRes.data.updatedAt),
  logoUrl:this.generateInitialsLogo(companyRes.data.name),
  users: []
};

            this.isLoading = false;
          },
          error: () => {
            this.toast.show({ title: 'Error', message: 'Company not found.' });
            this.isLoading = false;
          }
        });
      },
      error: () => {
        this.toast.show({ title: 'Error', message: 'Authorization error.' });
        this.isLoading = false;
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

}
