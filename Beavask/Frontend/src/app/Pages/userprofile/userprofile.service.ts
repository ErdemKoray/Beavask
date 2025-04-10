import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserprofileService {
  private baseUrl = 'https://localhost:5001/api/User';

  constructor(private http: HttpClient) {}

  getUserProfile(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/GetUserProfile/${id}`);
  }
}
