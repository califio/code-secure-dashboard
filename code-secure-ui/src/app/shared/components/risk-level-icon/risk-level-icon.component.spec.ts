import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RiskLevelIconComponent } from './risk-level-icon.component';

describe('RiskLevelComponent', () => {
  let component: RiskLevelIconComponent;
  let fixture: ComponentFixture<RiskLevelIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RiskLevelIconComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RiskLevelIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
