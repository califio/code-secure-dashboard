import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarMobileComponent } from './navbar-mobile.component';

describe('NavbarMobileComponent', () => {
  let component: NavbarMobileComponent;
  let fixture: ComponentFixture<NavbarMobileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavbarMobileComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavbarMobileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
