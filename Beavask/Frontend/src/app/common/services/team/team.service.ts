import { Injectable } from '@angular/core';
import { GenericHttpsService } from '../generic-https.service';
import { Team } from '../../model/team.model';

@Injectable({
  providedIn: 'root'
})
export class TeamService {

 private endpoint = 'Team';

  constructor(private apiService: GenericHttpsService<Team>) {}

  getAll() {
    return this.apiService.getAll(this.endpoint);
  }

  getById(id: number) {
    return this.apiService.getById(this.endpoint, id);
  }

  create(auth: Team) {
    return this.apiService.create(this.endpoint, auth);
  }

  update(id: number, auth: Team) {
    return this.apiService.update(this.endpoint, id, auth);
  }

  delete(id: number) {
    return this.apiService.delete(this.endpoint, id);
  }
}
