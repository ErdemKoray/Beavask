import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../model/apiResponse.model';
import { Friend } from '../friendship/friendship.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectUserService {
  private endpoint = "http://localhost:5092/api/Project"; 

  constructor(private _http:HttpClient) { }

  getProjectUser(id:number):Observable<ApiResponse<Friend[]>>{
    return this._http.get<ApiResponse<Friend[]>>(`${this.endpoint}/get-all-users-by-project-id/?projectId=${id}`)
  }
}
