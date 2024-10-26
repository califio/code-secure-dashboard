import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NgListAssetComponent } from './ng-list-asset.component';

describe('NgListAssetComponent', () => {
  let component: NgListAssetComponent;
  let fixture: ComponentFixture<NgListAssetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NgListAssetComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NgListAssetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
