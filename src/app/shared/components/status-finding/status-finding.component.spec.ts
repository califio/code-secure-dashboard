import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatusFindingComponent } from './status-finding.component';

describe('StatusFindingComponent', () => {
  let component: StatusFindingComponent;
  let fixture: ComponentFixture<StatusFindingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StatusFindingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StatusFindingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
