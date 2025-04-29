import { Routes } from '@angular/router';

import { AuthGuard } from './common/interceptor/authguard';

export const routes: Routes = [

    {
        path: '',
        loadComponent:()=>import('./Pages/home/home.component').then(m=>m.HomeComponent),
        children: [
            {
                path: '',
                loadComponent:()=>import('./Pages/home/main/main.component').then(m=>m.MainComponent),
                canActivate: [AuthGuard]
            },
            {
                path: 'userprofile',
                loadComponent:()=>import('./Pages/userprofile/userprofile.component').then(m=>m.UserprofileComponent),
                canActivate: [AuthGuard]
            },
            {
                path: 'myactivities',
                loadComponent:()=>import('./Pages/home/my-dashboard/my-dashboard.component').then(m=>m.MyDashboardComponent),
                canActivate: [AuthGuard]
            },
            {
                path: 'mytasks',
                loadComponent:()=>import('./Pages/home/my-task/my-task.component').then(m=>m.MyTaskComponent),
                canActivate: [AuthGuard]
            },
             {
                path: 'teams/:id',
                loadComponent: () => import('./Pages/teamprofile/teamprofile.component').then(m => m.TeamprofileComponent),
                canActivate: [AuthGuard]
            }
,
            {
                path:'company',
                loadComponent:()=>import('./Pages/Company/company.component').then(m=>m.CompanyComponent),
               canActivate: [AuthGuard] ,
                children:[
                    {
                        path:'profile',
                        loadComponent:()=>import('./Pages/Company/cprofile/cprofile.component').then(m=>m.CprofileComponent),
                       canActivate: [AuthGuard] 
                    }
                ]
            },
            {
                path:'projects',
                loadComponent:()=>import('./Pages/home/projects/projects.component').then(m=>m.ProjectsComponent),
              
            },
            {
                path: 'project-detail',
                loadComponent:()=>import('./Pages/home/projects/project-details/project-details.component').then(m=>m.ProjectDetailsComponent),
               canActivate: [AuthGuard],
                children:[
                    {
                        path:"board",
                        loadComponent:()=>import('./Pages/home/projects/project-details/project-detail/board/board.component').then(m=>m.BoardComponent),
                       canActivate: [AuthGuard] 
                    },
                    {
                        path:"summary",
                        loadComponent:()=>import('./Pages/home/projects/project-details/project-detail/summary/summary.component').then(m=>m.SummaryComponent),
                       canActivate: [AuthGuard] 
                    },
                    {
                        path:"timeline",
                        loadComponent:()=>import('./Pages/home/projects/project-details/project-detail/timeline/timeline.component').then(m=>m.TimelineComponent),
                       canActivate: [AuthGuard] 
                    }
                ]
            }

        ]
    },
    {
        path: 'login',
        loadComponent:()=>import('./Pages/auth/login/login.component').then(m=>m.LoginComponent)
    },
    {
        path: 'register',
        loadComponent:()=>import('./Pages/auth/register/register.component').then(m=>m.RegisterComponent)
    },
    {
        path: 'auth-callback',
    loadComponent:()=>import('./Pages/auth/auth-callback/auth-callback.component').then(m=>m.AuthCallbackComponent)
    },

    {
        path:"**",
        loadComponent:()=>import('./components/not-found/not-found.component').then(m=>m.NotFoundComponent)
    }
];
