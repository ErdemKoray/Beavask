import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppComponent } from '../../app.component';

@Injectable({
  providedIn: 'root'
})
export class GenericHttpsService {

  constructor(
    private _http:HttpClient,
    private _app:AppComponent

  ) { 
  }

  private baseUrl = 'https://jsonplaceholder.typicode.com';

  get(url: string) {
    return this._http.get(this.baseUrl + url);
  }
}
