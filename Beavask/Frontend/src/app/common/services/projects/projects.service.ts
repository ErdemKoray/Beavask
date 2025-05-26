import { Injectable } from '@angular/core';
import { GenericHttpsService } from '../generic-https.service';
import { Project } from '../../model/project.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {


  constructor(private apiService:GenericHttpsService<Project>) { }

  private endpoint = "Project";

  getAll(){
    return this.apiService.getAll(`${this.endpoint}/get-all-projects-by-user`);
  }
  getById(id: number) {
    return this.apiService.getById(this.endpoint, id);
  }


  update(id: number, auth: Project) {
    return this.apiService.update(this.endpoint, id, auth);
  }

  delete(id: number) {
    return this.apiService.delete(this.endpoint, id);
  }
}
