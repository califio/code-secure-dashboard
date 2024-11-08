import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FindingStatusComponent } from './finding-status.component';

describe('StatusFindingComponent', () => {
  let component: FindingStatusComponent;
  let fixture: ComponentFixture<FindingStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FindingStatusComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FindingStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
