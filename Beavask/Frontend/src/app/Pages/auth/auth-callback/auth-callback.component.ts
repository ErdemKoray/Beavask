import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../common/services/auth/auth.service';
import { ToastService } from '../../../components/toast/toast.service';
import { UserService, UserInfo } from '../../../common/services/user.service';
import { AuthprofileService } from '../../../common/services/profile/authprofile.service';

@Component({
  selector: 'app-auth-callback',
  standalone: true,
  imports: [],
  templateUrl: './auth-callback.component.html',
  styleUrl: './auth-callback.component.css'
})
export class AuthCallbackComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private authprofileService: AuthprofileService,
    private userService: UserService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const code = params['code'];
      const companyIdParam = params['companyId'];

      if (!code) {
        this.toastService.show({ title: 'Error', message: 'Authorization code not found.' });
        this.router.navigate(['/login']);
        return;
      }

      this.authService.githubLogin(code).subscribe({
        next: (response) => {
          const token = response.data;
          if (!token) {
            this.toastService.show({ title: 'Error', message: 'Token not received from server.' });
            this.router.navigate(['/login']);
            return;
          }

          // Token'ı kaydet
          localStorage.setItem('jwtToken', token);

          // Profil bilgisi token ile alınacak
          this.authprofileService.whoami().subscribe({
            next: (profile) => {
              const userId = profile.userId.toString();

              if (companyIdParam && userId) {
                this.userService.getUserById(userId).subscribe({
                  next: (userInfo) => {
                    if (userInfo.companyId !== +companyIdParam) {
                      const cId=Number(+companyIdParam)
 this.userService.updateCompanyId(userId, cId).subscribe({
  next: () => {
    this.toastService.show({ title: 'Success', message: 'Company info updated!' });
    this.router.navigate(['/']);
  },
  error: (err) => {
    this.toastService.show({ title: 'Error', message: err?.error?.message || 'Failed to update company info.' });
    this.router.navigate(['/']);
  }
});

} else {
  this.router.navigate(['/']);
}

                  },
                  error: () => {
                    this.toastService.show({ title: 'Error', message: 'Failed to fetch user info.' });
                    this.router.navigate(['/']);
                  }
                });
              } else {
                this.router.navigate(['/']);
              }
            },
            error: () => {
              this.toastService.show({ title: 'Error', message: 'Failed to fetch profile.' });
              this.router.navigate(['/login']);
            }
          });
        },
        error: () => {
          this.toastService.show({ title: 'Error', message: 'Login failed!' });
          this.router.navigate(['/login']);
        }
      });
    });
  }
}
