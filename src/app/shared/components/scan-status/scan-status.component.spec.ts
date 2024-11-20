import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScanStatusComponent } from './scan-status.component';

describe('ScanStatusComponent', () => {
  let component: ScanStatusComponent;
  let fixture: ComponentFixture<ScanStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ScanStatusComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ScanStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
