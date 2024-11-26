import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopDependencyChartComponent } from './top-dependency-chart.component';

describe('TopDependencyChartComponent', () => {
  let component: TopDependencyChartComponent;
  let fixture: ComponentFixture<TopDependencyChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopDependencyChartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TopDependencyChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
