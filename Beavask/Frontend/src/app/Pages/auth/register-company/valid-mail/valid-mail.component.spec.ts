import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidMailComponent } from './valid-mail.component';

describe('ValidMailComponent', () => {
  let component: ValidMailComponent;
  let fixture: ComponentFixture<ValidMailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ValidMailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ValidMailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
