import { Injectable } from '@angular/core';
import { GenericHttpsService } from '../generic-https.service';
import { Team } from '../../model/team.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../model/apiResponse.model';
import { companyTeam } from './model/company-team.model';
import { teamMember } from './model/teamMember.model';

@Injectable({
  providedIn: 'root'
})
export class TeamService {

   private endpoint = "http://localhost:5092/api/Team"; 
   constructor(private _http:HttpClient) { }
   getTeamInfo(teamId:Number):Observable<ApiResponse<companyTeam>>{
    return this._http.get<ApiResponse<companyTeam>>(`${this.endpoint}/${teamId}`)
   }
   deleteTeamInfo(teamId:Number):Observable<ApiResponse<string>>{
    return this._http.delete<ApiResponse<string>>(`${this.endpoint}/${teamId}`)
   }
  getById(teamId:Number):Observable<ApiResponse<Team>>{
    return this._http.get<ApiResponse<Team>>(`${this.endpoint}/${teamId}`)
   }
  getAll() :Observable<ApiResponse<Team[]>>{
    return this._http.get<ApiResponse<Team[]>>(`${this.endpoint}`)
   }
   getCompanyTeam(id:Number):Observable<ApiResponse<companyTeam[]>>{
    return this._http.get<ApiResponse<companyTeam[]>>(`${this.endpoint}/company/${id}/teams`)
   }

getTeamMember(teamId: number): Observable<ApiResponse<companyTeam>> {
  return this._http.get<ApiResponse<companyTeam>>(`${this.endpoint}/${teamId}/members`);
}

createTeam(teamName: string): Observable<ApiResponse<string>> {
  return this._http.post<ApiResponse<string>>(
    `${this.endpoint}/company/create-team`,
    { title: teamName }  // JSON obje olarak gönderiyoruz
  );
}

     assignUserToTeam(teamId:Number,userId:Number):Observable<ApiResponse<string>>{
    return this._http.post<ApiResponse<string>>(`${this.endpoint}/${teamId}/assign-user/${userId}`,{teamId,userId})
   }

}
