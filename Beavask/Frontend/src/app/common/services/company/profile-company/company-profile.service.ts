import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CompanyProfile } from './model/companyProfile.model';
import { ApiResponse } from '../../../model/apiResponse.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyProfileService {


    private baseUrl = 'http://localhost:5092/api/Profile';

  constructor(private http: HttpClient) {}

  whoamiCompany(): Observable<CompanyProfile> {
    return this.http.get<CompanyProfile>(`${this.baseUrl}/whoami-company`);
  }
}
