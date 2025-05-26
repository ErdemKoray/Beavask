import { Injectable } from '@angular/core';
import { GenericHttpsService } from '../generic-https.service';
import { cProject } from './creatproject.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../model/apiResponse.model';

@Injectable({
  providedIn: 'root'
})


export class CreatprojectService {

  private endpoint = "http://localhost:5092/api/Project/create-from-github"; 
  constructor(private _http:HttpClient) { }

create(auth: cProject): Observable<string> {
  return this._http.post(this.endpoint, auth, { responseType: 'text' });
}

}
