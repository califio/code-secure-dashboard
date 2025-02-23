import {ComponentFixture, TestBed} from '@angular/core/testing';

import {TopFindingChartComponent} from './top-finding-chart.component';

describe('TopFindingCategoryComponent', () => {
  let component: TopFindingChartComponent;
  let fixture: ComponentFixture<TopFindingChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopFindingChartComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(TopFindingChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
