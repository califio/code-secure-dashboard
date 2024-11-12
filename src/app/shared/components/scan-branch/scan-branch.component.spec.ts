import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScanBranchComponent } from './scan-branch.component';

describe('ScanBranchComponent', () => {
  let component: ScanBranchComponent;
  let fixture: ComponentFixture<ScanBranchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ScanBranchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ScanBranchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
