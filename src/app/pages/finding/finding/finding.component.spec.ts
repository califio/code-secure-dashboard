import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FindingComponent } from './finding.component';

describe('FindingComponent', () => {
  let component: FindingComponent;
  let fixture: ComponentFixture<FindingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FindingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FindingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
