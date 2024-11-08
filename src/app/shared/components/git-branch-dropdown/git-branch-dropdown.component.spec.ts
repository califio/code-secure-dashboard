import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GitBranchDropdownComponent } from './git-branch-dropdown.component';

describe('GitBranchDropdownComponent', () => {
  let component: GitBranchDropdownComponent;
  let fixture: ComponentFixture<GitBranchDropdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GitBranchDropdownComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GitBranchDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
