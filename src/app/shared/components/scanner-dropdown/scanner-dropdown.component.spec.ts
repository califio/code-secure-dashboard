import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScannerDropdownComponent } from './scanner-dropdown.component';

describe('ScannerDropdownComponent', () => {
  let component: ScannerDropdownComponent;
  let fixture: ComponentFixture<ScannerDropdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ScannerDropdownComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ScannerDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
