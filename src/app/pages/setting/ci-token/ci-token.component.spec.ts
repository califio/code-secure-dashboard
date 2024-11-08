import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CiTokenComponent } from './ci-token.component';

describe('CiTokenComponent', () => {
  let component: CiTokenComponent;
  let fixture: ComponentFixture<CiTokenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CiTokenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CiTokenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
