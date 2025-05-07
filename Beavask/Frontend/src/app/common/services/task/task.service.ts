import { Injectable } from '@angular/core';
import { GenericHttpsService } from '../generic-https.service';
import { Task } from '../../model/task.model';
import { HttpClient } from '@angular/common/http';
import { CreateTaskModel } from './taskModel/createTask.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private _http:HttpClient) { }

  private endpoint = "task";

    create(model: CreateTaskModel) {
      return this._http.post<any>(this.endpoint, model);
    }
    getAll() {
      return this._http.get<Task[]>(this.endpoint);
    }

    getById(id: number) {
      return this._http.get<Task>(`${this.endpoint}/${id}`);
    }

    
}
