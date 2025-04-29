import { Injectable } from '@angular/core';
import { GenericHttpsService } from '../generic-https.service';
import { GithubRepo } from '../../model/githubrepo.model';

@Injectable({
  providedIn: 'root'
})
export class GithubrepoService {
  private endpoint = "Repo/my-public-repos";
  constructor(private apiServices:GenericHttpsService<GithubRepo> ) { }

  getGithubRepos(){
    return this.apiServices.getAll(this.endpoint)
  }

}
