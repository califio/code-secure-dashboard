import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IntegrationComponent } from './integration.component';

describe('IntegrationComponent', () => {
  let component: IntegrationComponent;
  let fixture: ComponentFixture<IntegrationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IntegrationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IntegrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
