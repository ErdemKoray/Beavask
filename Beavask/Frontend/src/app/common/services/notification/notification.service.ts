import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../model/apiResponse.model';


 
  export interface notification{
      id: number
      notificationType: string
      title: string
      content: string
      createdAt:string
      userId: number

  }
@Injectable({
  providedIn: 'root'
})
export class NotificationService {
    private baseUrl = 'http://localhost:5092/api/Notification';

  constructor(private _http:HttpClient) { }
  
  getAll():Observable<ApiResponse<notification>>{
    return this._http.get<ApiResponse<notification>>(`${this.baseUrl}`);
  }
}
