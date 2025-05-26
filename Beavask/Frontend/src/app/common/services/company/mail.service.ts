import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../model/apiResponse.model';
import { CompanyMail } from './profile-company/model/company-mail.model';

@Injectable({
  providedIn: 'root'
})


export class MailService {
  private baseUrl = 'http://localhost:5092/api/Mail/send-project-invitation';

   constructor(private http: HttpClient) {}
  
   sendMail(auth: CompanyMail): Observable<string> {
     return this.http.post<string>(this.baseUrl, auth);
   }
   
}
