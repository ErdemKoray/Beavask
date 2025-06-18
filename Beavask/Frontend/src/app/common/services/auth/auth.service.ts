import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../model/apiResponse.model'; 
import { ToastService } from '../../../components/toast/toast.service';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'http://localhost:5092/api/Auth';

  constructor(private http: HttpClient) {}

  githubLogin(code: string): Observable<ApiResponse<string>> {  
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<ApiResponse<string>>(
      `${this.baseUrl}/github-login`,
      { code },
      { headers }
    );
  }
  register(auth:any): Observable<ApiResponse<string>> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<ApiResponse<string>>(
      `${this.baseUrl}/register`,
      auth,
      { headers }
    );
  }

  login(auth:any): Observable<ApiResponse<string>> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<ApiResponse<string>>(
      `${this.baseUrl}/login-personal`,
      auth,
      { headers }
    );
  }
sendMailToResetPassword(mail: string): Observable<string> {
  const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  // DOĞRU: { email: mail }
 return this.http.post<string>(
  `${this.baseUrl}/send-reset-password-email?email=${mail}`, {}, { headers }
);
}


VerifyMail(mail: string, code: string): Observable<ApiResponse<string>> {
  const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  return this.http.post<ApiResponse<string>>(
    `${this.baseUrl}/verify-reset-password?email=${mail}&code=${code}`,
    {}, // boş body
    { headers }
  );
}

changePassword1(oldPassword: string,newPassword: string,confirmNewPassword: string):Observable<ApiResponse<string>>{
  const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

  return this.http.post<ApiResponse<string>>(`${this.baseUrl}/change-user-password`,{oldPassword,newPassword,confirmNewPassword},{headers});
}

changePassword(mail: string, password: string, confirmPassword: string): Observable<ApiResponse<string>> {
  const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  return this.http.post<ApiResponse<string>>(
    `${this.baseUrl}/forgot-password`,
    {
      email: mail, // <-- email anahtar kelimesi!
      password,
      confirmPassword
    },
    { headers }
  );
}


csendMailToResetPassword(mail: string): Observable<string> {
  const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  // DOĞRU: { email: mail }
 return this.http.post<string>(
  `${this.baseUrl}/send-reset-password-email?email=${mail}`, {}, { headers }
);
}


cVerifyMail(mail: string, code: string): Observable<ApiResponse<string>> {
  const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  return this.http.post<ApiResponse<string>>(
    `${this.baseUrl}/verify-reset-password?email=${mail}&code=${code}`,
    {}, // boş body
    { headers }
  );
}



cchangePassword(mail: string, password: string, confirmPassword: string): Observable<ApiResponse<string>> {
  const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  return this.http.post<ApiResponse<string>>(
    `${this.baseUrl}/forgot-password`,
    {
      email: mail, 
      password,
      confirmPassword
    },
    { headers }
  );
}



}
