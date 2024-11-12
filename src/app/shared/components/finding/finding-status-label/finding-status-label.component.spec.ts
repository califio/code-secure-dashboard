import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FindingStatusLabelComponent } from './finding-status-label.component';

describe('FindingStatusLabelComponent', () => {
  let component: FindingStatusLabelComponent;
  let fixture: ComponentFixture<FindingStatusLabelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FindingStatusLabelComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FindingStatusLabelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
