import {ComponentFixture, TestBed} from '@angular/core/testing';

import {UpdateUserPopupComponent} from './update-user-popup.component';

describe('UpdateUserPopupComponent', () => {
  let component: UpdateUserPopupComponent;
  let fixture: ComponentFixture<UpdateUserPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateUserPopupComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(UpdateUserPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
