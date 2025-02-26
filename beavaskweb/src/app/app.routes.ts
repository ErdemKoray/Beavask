import { Routes } from '@angular/router';
import { HomeComponent } from './Pages/home/home.component';
import { LoginComponent } from './Pages/auth/login/login.component';

export const routes: Routes = [

    {
        path: '',
        component:HomeComponent
    },
    {
        path: 'login',
        component:LoginComponent
    }
];
