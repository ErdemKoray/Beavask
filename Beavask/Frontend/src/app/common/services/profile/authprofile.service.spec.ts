import { TestBed } from '@angular/core/testing';

import { AuthprofileService } from './authprofile.service';

describe('AuthprofileService', () => {
  let service: AuthprofileService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthprofileService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
