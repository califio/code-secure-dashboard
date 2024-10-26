import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListAssetComponent } from './list-asset.component';

describe('ListAssetComponent', () => {
  let component: ListAssetComponent;
  let fixture: ComponentFixture<ListAssetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListAssetComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListAssetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
