import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastComponent } from '../../components/toast/toast.component';
import { NavbarCompanyComponent } from './navbar-company/navbar-company.component';

@Component({
  selector: 'app-company',
  standalone: true,
  imports: [RouterOutlet,ToastComponent,NavbarCompanyComponent],
  templateUrl: './company.component.html',
  styleUrl: './company.component.css'
})
export class CompanyComponent {

}
