import { Injectable } from '@angular/core';
import { GenericHttpsService } from '../generic-https.service';

import { HttpClient } from '@angular/common/http';
import { CreateTaskModel } from './taskModel/createTask.model';
import { Task } from './taskModel/task.model';
import { ApiResponse } from '../../model/apiResponse.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private _http:HttpClient) { }

  private endpoint = "http://localhost:5092/api/Task";

    create(model: CreateTaskModel) {
      return this._http.post<any>(this.endpoint, model);
    }
    getAllTasks(projectId: number) {
      return this._http.get<ApiResponse<Task[]>>(`${this.endpoint}/project/${projectId}`);
    }

    getById(id: number) {
      return this._http.get<Task>(`${this.endpoint}/${id}`);
    }

    
}
