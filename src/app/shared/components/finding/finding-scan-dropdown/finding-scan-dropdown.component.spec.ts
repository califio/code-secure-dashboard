import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FindingScanDropdownComponent } from './finding-scan-dropdown.component';

describe('FindingScanDropdownComponent', () => {
  let component: FindingScanDropdownComponent;
  let fixture: ComponentFixture<FindingScanDropdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FindingScanDropdownComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FindingScanDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
