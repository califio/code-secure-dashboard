import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScanBranchDropdownComponent } from './scan-branch-dropdown.component';

describe('GitBranchDropdownComponent', () => {
  let component: ScanBranchDropdownComponent;
  let fixture: ComponentFixture<ScanBranchDropdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ScanBranchDropdownComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ScanBranchDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
