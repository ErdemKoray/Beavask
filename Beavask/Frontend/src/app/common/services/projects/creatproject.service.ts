import { Injectable } from '@angular/core';
import { GenericHttpsService } from '../generic-https.service';
import { cProject } from './creatproject.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CreatprojectService {

  private endpoint = "http://localhost:5092/api/Project/create-from-github"; 
  constructor(private _http:HttpClient) { }


  create(auth: cProject) {
    return this._http.post<any>(this.endpoint, auth);
  }
}
