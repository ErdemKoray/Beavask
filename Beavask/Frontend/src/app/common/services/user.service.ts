import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../model/user.model';
import { ApiResponse } from '../model/apiResponse.model';

export interface UserInfo {
  firstName: string;
  lastName: string;
  email: string;
  companyId: number;
  isActive: boolean;
}
export interface UserDetails {
    id:number;
    firstName: string;
    lastName: string;
    username:string;
   email: string;
    avatarUrl: string | null;
    createdAt:Date;
    updateAt:Date;
    isActive:boolean;
    teamId: number | null;
    companyId: number | null;
  

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
  getUserByIdn(userId: number): Observable<ApiResponse<UserDetails>> {
    return this.http.get<ApiResponse<UserDetails>>(`${this.baseUrl}/${userId}`);
  }

updateCompanyId(userId: string, companyId: number): Observable<void> {
  return this.http.put<void>(`${this.baseUrl}/company/${userId}`, { companyId });
}

apiUserByUsername(username:string):Observable<UserDetails>{
  return this.http.get<UserDetails>(`${this.baseUrl}/username/${username}`);

}
apiUserById(id:number):Observable<ApiResponse<UserDetails>>{
  return this.http.get<ApiResponse<UserDetails>>(`${this.baseUrl}/id/${id}`);

}

}
