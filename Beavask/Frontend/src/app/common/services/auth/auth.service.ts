import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericHttpsService } from '../generic-https.service';


import { auth } from '../../model/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private endpoint = 'users';

  constructor(private apiService: GenericHttpsService<auth>) {}

  getAll() {
    return this.apiService.getAll(this.endpoint);
  }

  getById(id: number) {
    return this.apiService.getById(this.endpoint, id);
  }

  create(auth: auth) {
    return this.apiService.create(this.endpoint, auth);
  }

  update(id: number, auth: auth) {
    return this.apiService.update(this.endpoint, id, auth);
  }

  delete(id: number) {
    return this.apiService.delete(this.endpoint, id);
  }
  
}
