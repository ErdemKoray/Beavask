import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Profile } from './profile.model';


@Injectable({
  providedIn: 'root'
})
export class AuthprofileService {

  private baseUrl = 'http://localhost:5092/api/Profile/whoami'; 

  constructor(private http: HttpClient) {}

  whoami(): Observable<Profile> { 
    return this.http.get<Profile>( this.baseUrl);
  }
}
