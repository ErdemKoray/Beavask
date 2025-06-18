import { Routes } from '@angular/router';

import { AuthGuard } from './common/interceptor/authguard';

export const routes: Routes = [
{
  path: 'company',
  loadComponent:()=>import('./Pages/Company/company.component').then(m=>m.CompanyComponent),
  canActivate: [AuthGuard],
  children:[
     {
      path:'dashboard',
      loadComponent:()=>import('./Pages/Company/company-dashboard/company-dashboard.component').then(m=>m.CompanyDashboardComponent),
      canActivate: [AuthGuard]
    },{
      path:'profile',
      loadComponent:()=>import('./Pages/Company/cprofile/cprofile.component').then(m=>m.CprofileComponent),
      canActivate: [AuthGuard]
    }, {
      path:'teams',
      loadComponent:()=>import('./Pages/Company/company-team/company-team.component').then(m=>m.CompanyTeamComponent),
      canActivate: [AuthGuard]
    }, {
      path:'projects',
      loadComponent:()=>import('./Pages/Company/company-projects/company-projects.component').then(m=>m.CompanyProjectsComponent),
      canActivate: [AuthGuard]
    }, {
      path:'project-detail/:id',
      loadComponent:()=>import('./Pages/Company/company-detail/company-detail.component').then(m=>m.CompanyDetailComponent),
      canActivate: [AuthGuard]
    }
  ]
}
,
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
                        path:'connections',
                        loadComponent:()=>import('./Pages/userprofile/user-connection/user-connection.component').then(m=>m.UserConnectionComponent),
                        canActivate:[AuthGuard]
                    } 
            ,
            {
            path: 'userprofile/:id',
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
                        path:"board/:projectId",
                        loadComponent:()=>import('./Pages/home/projects/project-details/project-detail/board/board.component').then(m=>m.BoardComponent),
                       canActivate: [AuthGuard] 
                    },
                    {
                        path:"summary",
                        loadComponent:()=>import('./Pages/home/projects/project-details/project-detail/summary/summary.component').then(m=>m.SummaryComponent),
                       canActivate: [AuthGuard] 
                    },
                    {
                        path:"timeline/:projectId",
                        loadComponent:()=>import('./Pages/home/projects/project-details/project-detail/timeline/timeline.component').then(m=>m.TimelineComponent),
                       canActivate: [AuthGuard] 
                    }
                ]
            }

        ]
    },
     {
                path:'main',
                loadComponent:()=>import('./Pages/homepage/homepage.component').then(m=>m.HomepageComponent),

            }
            ,
            
    {
        path: 'login',
        loadComponent:()=>import('./Pages/auth/login/login.component').then(m=>m.LoginComponent)
    },
    {
        path: 'register',
        loadComponent:()=>import('./Pages/auth/register/register.component').then(m=>m.RegisterComponent)
        
    },
    {
          path: 'rcompany',
        loadComponent:()=>import('./Pages/auth/register-company/register-company.component').then(m=>m.RegisterCompanyComponent),
      
    }, 
    {
            path:'verify',
                  loadComponent:()=>import('./Pages/auth/register-company/valid-mail/valid-mail.component').then(m=>m.ValidMailComponent),

        },{
          path: 'lcompany',
        loadComponent:()=>import('./Pages/auth/login-company/login-company.component').then(m=>m.LoginCompanyComponent)
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
