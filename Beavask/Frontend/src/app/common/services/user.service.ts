import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface UserInfo {
  firstName: string;
  lastName: string;
  email: string;
  companyId: number;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = 'http://localhost:5092/api/User';

  constructor(private http: HttpClient) {}

  getUserById(userId: string): Observable<UserInfo> {
    return this.http.get<UserInfo>(`${this.baseUrl}/${userId}`);
  }

updateCompanyId(userId: string, companyId: number): Observable<void> {
  return this.http.put<void>(`${this.baseUrl}/company/${userId}`, { companyId });
}



}
