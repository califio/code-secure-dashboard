import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FindingActivityComponent } from './finding-activity.component';

describe('FindingActivityComponent', () => {
  let component: FindingActivityComponent;
  let fixture: ComponentFixture<FindingActivityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FindingActivityComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FindingActivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
