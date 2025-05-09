import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../model/apiResponse.model'; 
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
  
}
