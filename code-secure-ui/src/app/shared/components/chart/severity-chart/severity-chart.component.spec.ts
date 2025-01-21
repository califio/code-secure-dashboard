import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SeverityChartComponent } from './severity-chart.component';

describe('SeverityChartComponent', () => {
  let component: SeverityChartComponent;
  let fixture: ComponentFixture<SeverityChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SeverityChartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SeverityChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
