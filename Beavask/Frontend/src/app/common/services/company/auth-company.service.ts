import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../model/apiResponse.model';
import { CompanyRegisterModel } from './model/createcompany.model';

@Injectable({
  providedIn: 'root'
})


export class AuthCompanyService {

  baseUrl="http://localhost:5092/api/Auth"
  constructor(private _http:HttpClient) { }


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
verifyMail(email: string, code: string): Observable<any> {
  return this._http.post<string>(
    `${this.baseUrl}/verify-company-email`,
    { email, code },
    {    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    responseType: 'text' as 'json'  }
  );
}

}
