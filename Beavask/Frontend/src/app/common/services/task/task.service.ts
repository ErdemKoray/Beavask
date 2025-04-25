import { Injectable } from '@angular/core';
import { GenericHttpsService } from '../generic-https.service';
import { Task } from '../../model/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private apiService:GenericHttpsService<Task>) { }

  private endpoint = "task";

  getAll(){
    return this.apiService.getAll(this.endpoint);
  }
  getById(id: number) {
    return this.apiService.getById(this.endpoint, id);
  }

  create(auth: Task) {
    return this.apiService.create(this.endpoint, auth);
  }

  update(id: number, auth: Task) {
    return this.apiService.update(this.endpoint, id, auth);
  }

  delete(id: number) {
    return this.apiService.delete(this.endpoint, id);
  }
}
