import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../model/apiResponse.model';
import { Company } from './company.model';
import { CompanyUser } from './profile-company/company.model';
import { CompanyProject } from './model/companyProject.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private baseUrl = 'http://localhost:5092/api/Company';

  constructor(private http: HttpClient) {}

  getCompanyById(id: number): Observable<ApiResponse<Company>> {
    return this.http.get<ApiResponse<Company>>(`${this.baseUrl}/${id}`);
  }

  updateCompany(id: number, model: Company): Observable<ApiResponse<Company>> {
    return this.http.put<ApiResponse<Company>>(`${this.baseUrl}/${id}`, model);
  }

  deleteCompany(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.baseUrl}/${id}`);
  }

  getCompanyUsers(companyId: number): Observable<CompanyUser[]> {
    return this.http.get<CompanyUser[]>(`${this.baseUrl}/Company/projects/${companyId}/members`);
  }

  getCompanyProjects(): Observable<CompanyProject[]> {
  return this.http.get<CompanyProject[]>(`${this.baseUrl}/Company/projects`);
  }

  getCompanyAllUsers(companyId: number): Observable<CompanyUser[]> {
  return this.http.get<CompanyUser[]>(`${this.baseUrl}/${companyId}/users`);
  }

}
