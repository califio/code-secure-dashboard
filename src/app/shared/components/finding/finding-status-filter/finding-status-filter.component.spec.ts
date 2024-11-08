import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FindingStatusFilterComponent } from './finding-status-filter.component';

describe('FindingStatusFilterComponent', () => {
  let component: FindingStatusFilterComponent;
  let fixture: ComponentFixture<FindingStatusFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FindingStatusFilterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FindingStatusFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
