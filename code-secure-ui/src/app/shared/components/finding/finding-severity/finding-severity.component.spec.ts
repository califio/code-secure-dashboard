import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FindingSeverityComponent } from './finding-severity.component';

describe('FindingSeverityComponent', () => {
  let component: FindingSeverityComponent;
  let fixture: ComponentFixture<FindingSeverityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FindingSeverityComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FindingSeverityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
