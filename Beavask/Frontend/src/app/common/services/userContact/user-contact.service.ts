import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserContact } from './model/userContact.model';
import { ApiResponse } from '../../model/apiResponse.model';
export interface UserContactRequest {
  contactType: string;
  contactValue: string;
  userId: number;
}

@Injectable({
  providedIn: 'root'
})
export class UserContactService {

  private baseUrl = 'http://localhost:5092/api/UserContact';

  constructor(private http: HttpClient) {}

  getContactById(userId: number): Observable<ApiResponse<UserContact[]>> {
    return this.http.get<ApiResponse<UserContact[]>>(`${this.baseUrl}/user/${userId}`);
  }
   postContactById(model: UserContactRequest): Observable<string> {
    return this.http.post<string>(`${this.baseUrl}`,model);
  }

   deleteContactById(userId:number): Observable<string> {
    return this.http.delete<string>(`${this.baseUrl}/${userId}`);
  }
    updateContactById(model: UserContactRequest,id:number): Observable<string> {
    return this.http.put<string>(`${this.baseUrl}/${id}`,model);
  }
}
