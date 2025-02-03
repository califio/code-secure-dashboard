import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketDropdownComponent } from './ticket-dropdown.component';

describe('TicketDropdownComponent', () => {
  let component: TicketDropdownComponent;
  let fixture: ComponentFixture<TicketDropdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicketDropdownComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TicketDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
