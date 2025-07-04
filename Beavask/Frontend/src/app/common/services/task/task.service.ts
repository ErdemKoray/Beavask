import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CreateTaskModel } from './taskModel/createTask.model';
import { Task } from './taskModel/task.model';
import { ApiResponse } from '../../model/apiResponse.model';
import { UpdateTaskModel } from './taskModel/updateTask.model';
import { Observable } from 'rxjs';

export interface UserInfos {
   firstName:string
    lastName: string
    username: string
    email: string
    avatarUrl: string
    createdAt: Date|null
    updatedAt: Date|null
    isActive:boolean
    teamId: number
    companyId: number
}
@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private _http:HttpClient) { }

  private endpoint = "http://localhost:5092/api/Task";

    create(model: CreateTaskModel) {
      return this._http.post<any>(this.endpoint, model);
    }
    getAllTasks(projectId: number):Observable<ApiResponse<Task[]>> {
      return this._http.get<ApiResponse<Task[]>>(`${this.endpoint}/project/${projectId}`);
    }

    getById(id: number) {
      return this._http.get<Task>(`${this.endpoint}/${id}`);
    }

    assignTaskToUser(taskId:number,userId:number){
      return this._http.post<any>(`${this.endpoint}/${taskId}/assign/${userId}`,{taskId,userId})
    }

    getReporter(id:number):Observable<ApiResponse<UserInfos>>{
      console.log(id)
      return this._http.get<ApiResponse<UserInfos>>(`http://localhost:5092/api/User/id/${id}`);
    }

    updateTask(id:number, model:UpdateTaskModel){
      return this._http.put<any>(`${this.endpoint}/${id}`,model)
    }

    deleteTask(id:number){
      return this._http.delete<any>(`${this.endpoint}/${id}`)
    }
    getUserTaskById(id:number){
      return this._http.get<ApiResponse<Task[]>>(`${this.endpoint}/user/${id}`)
    }
}
