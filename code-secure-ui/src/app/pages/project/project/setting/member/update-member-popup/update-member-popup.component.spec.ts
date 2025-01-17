import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateMemberPopupComponent } from './update-member-popup.component';

describe('UpdateMemberPopupComponent', () => {
  let component: UpdateMemberPopupComponent;
  let fixture: ComponentFixture<UpdateMemberPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateMemberPopupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateMemberPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
