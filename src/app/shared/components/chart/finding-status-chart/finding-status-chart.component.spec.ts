import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FindingStatusChartComponent } from './finding-status-chart.component';

describe('FindingStatusChartComponent', () => {
  let component: FindingStatusChartComponent;
  let fixture: ComponentFixture<FindingStatusChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FindingStatusChartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FindingStatusChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
