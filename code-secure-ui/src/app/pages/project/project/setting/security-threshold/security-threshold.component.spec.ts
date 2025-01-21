import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecurityThresholdComponent } from './security-threshold.component';

describe('SecurityThresholdComponent', () => {
  let component: SecurityThresholdComponent;
  let fixture: ComponentFixture<SecurityThresholdComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SecurityThresholdComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SecurityThresholdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
