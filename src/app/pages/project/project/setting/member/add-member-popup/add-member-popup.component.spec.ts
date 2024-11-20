import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMemberPopupComponent } from './add-member-popup.component';

describe('AddMemberPopupComponent', () => {
  let component: AddMemberPopupComponent;
  let fixture: ComponentFixture<AddMemberPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddMemberPopupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddMemberPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
