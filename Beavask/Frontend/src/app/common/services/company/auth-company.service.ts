import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../model/apiResponse.model';
import { CompanyRegisterModel } from './model/createcompany.model';
import { Company } from './company.model';
import { CompanyInfo } from './profile-company/model/companyInfo.model';

@Injectable({
  providedIn: 'root'
})


export class AuthCompanyService {

  baseUrl="http://localhost:5092/api/Auth"
  constructor(private _http:HttpClient) { }

// company-auth.service.ts

getWhoamiCompany(): Observable<CompanyInfo> {
  return this._http.get<CompanyInfo>('http://localhost:5092/api/Profile/whoami-company');
}

createCompany(model: CompanyRegisterModel): Observable<string> {
 return this._http.post<string>(
  `${this.baseUrl}/company-register`,
  model,
  {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    responseType: 'text' as 'json' 
  }
);
}



  login(model:any): Observable<ApiResponse<string>> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this._http.post<ApiResponse<string>>(
      `${this.baseUrl}/login-company`,
      model,
      { headers }
    );
  }
verifyMail(email: string, code: string): Observable<string> {
  const params = { email, code };
  return this._http.post(
    `${this.baseUrl}/verify-company-email`,
    null,
    {
      params,
      responseType: 'text' // düz metin dönerse bu gerekli
    }
  );
}



}
