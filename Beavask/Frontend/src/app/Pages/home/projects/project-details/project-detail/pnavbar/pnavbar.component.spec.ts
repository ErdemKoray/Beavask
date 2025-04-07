import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PnavbarComponent } from './pnavbar.component';

describe('PnavbarComponent', () => {
  let component: PnavbarComponent;
  let fixture: ComponentFixture<PnavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PnavbarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PnavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
