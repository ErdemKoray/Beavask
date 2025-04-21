import { TestBed } from '@angular/core/testing';

import { GenericHttpsService } from './generic-https.service';

describe('GenericHttpsService', () => {
  let service: GenericHttpsService<any>;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GenericHttpsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
