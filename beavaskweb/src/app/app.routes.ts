import { Routes } from '@angular/router';

export const routes: Routes = [

    {
        path: '',
        loadComponent:()=>import('./Pages/home/home.component').then(m=>m.HomeComponent),
        children: [
            {
                path: '',
                loadComponent:()=>import('./Pages/home/main/main.component').then(m=>m.MainComponent)
            },
            {
                path: 'userprofile',
                loadComponent:()=>import('./Pages/userprofile/userprofile.component').then(m=>m.UserprofileComponent)
            },
            {
                path:'projects',
                loadComponent:()=>import('./Pages/home/projects/projects.component').then(m=>m.ProjectsComponent),
              
            },
            {
                path: 'project-detail',
                loadComponent:()=>import('./Pages/home/projects/project-details/project-details.component').then(m=>m.ProjectDetailsComponent),
                children:[
                    {
                        path:"board",
                        loadComponent:()=>import('./Pages/home/projects/project-details/project-detail/board/board.component').then(m=>m.BoardComponent)
                    },
                    {
                        path:"summary",
                        loadComponent:()=>import('./Pages/home/projects/project-details/project-detail/summary/summary.component').then(m=>m.SummaryComponent)
                    },
                    {
                        path:"timeline",
                        loadComponent:()=>import('./Pages/home/projects/project-details/project-detail/timeline/timeline.component').then(m=>m.TimelineComponent)
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
    }
];
