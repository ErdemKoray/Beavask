import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyTeamComponent } from './company-team.component';

describe('CompanyTeamComponent', () => {
  let component: CompanyTeamComponent;
  let fixture: ComponentFixture<CompanyTeamComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CompanyTeamComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CompanyTeamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
