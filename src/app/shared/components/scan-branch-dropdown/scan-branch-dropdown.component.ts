import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {ClickOutsideDirective} from '../../directives/click-outside.directive';
import {NgClass} from '@angular/common';
import {ProjectCommitSummary} from '../../../api/models/project-commit-summary';
import {getGitActionIcon} from '../../helper';
import {GitAction} from '../../../api/models';

@Component({
  selector: 'scan-branch-dropdown',
  standalone: true,
  imports: [
    NgIcon,
    ClickOutsideDirective,
    NgClass
  ],
  templateUrl: './scan-branch-dropdown.component.html',
  styleUrl: './scan-branch-dropdown.component.scss'
})
export class ScanBranchDropdownComponent {
  hidden = true;
  label = 'Branch';
  selectedOption: ProjectCommitSummary | undefined;

  @Input()
  set selected(commitId: string | null | undefined) {
    this.selectedOption = this.options.find(option => option.commitId == commitId);
  }

  @Input()
  options: ProjectCommitSummary[] = [];
  @Output()
  selectChange = new EventEmitter<string>();

  onClick(option: ProjectCommitSummary) {
    this.selectedOption = option;
    this.selectChange.emit(option.commitId);
  }

  protected readonly getGitActionIcon = getGitActionIcon;
  protected readonly GitAction = GitAction;
}
