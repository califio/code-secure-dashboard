import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListDependencyComponent } from './list-dependency.component';

describe('ListDependencyComponent', () => {
  let component: ListDependencyComponent;
  let fixture: ComponentFixture<ListDependencyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListDependencyComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListDependencyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
