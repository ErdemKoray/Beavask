import { TestBed } from '@angular/core/testing';

import { GithubrepoService } from './githubrepo.service';

describe('GithubrepoService', () => {
  let service: GithubrepoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GithubrepoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
