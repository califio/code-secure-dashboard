import {Component, EventEmitter, Input, input, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {ClickOutsideDirective} from '../../directives/click-outside.directive';
import {NgClass} from '@angular/common';

@Component({
  selector: 'git-branch-dropdown',
  standalone: true,
  imports: [
    NgIcon,
    ClickOutsideDirective,
    NgClass
  ],
  templateUrl: './git-branch-dropdown.component.html',
  styleUrl: './git-branch-dropdown.component.scss'
})
export class GitBranchDropdownComponent {
  hidden = true;
  label = 'Branch';
  @Input()
  selected?: string | null = null;
  options = input<string[]>([])
  @Output()
  select = new EventEmitter<string>()
  onClick(option: string) {
    this.select.emit(option);
  }
}
