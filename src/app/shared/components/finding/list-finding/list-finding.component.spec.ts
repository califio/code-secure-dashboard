import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListFindingComponent } from './list-finding.component';

describe('ListFindingComponent', () => {
  let component: ListFindingComponent;
  let fixture: ComponentFixture<ListFindingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListFindingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListFindingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
