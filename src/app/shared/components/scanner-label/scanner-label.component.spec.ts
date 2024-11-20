import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScannerLabelComponent } from './scanner-label.component';

describe('ScannerLabelComponent', () => {
  let component: ScannerLabelComponent;
  let fixture: ComponentFixture<ScannerLabelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ScannerLabelComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ScannerLabelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
