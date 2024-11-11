import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {ClickOutsideDirective} from '../../directives/click-outside.directive';
import {NgClass} from '@angular/common';
import {GitAction} from '../../../api/models/git-action';
import {ProjectScanSummary} from '../../../api/models/project-scan-summary';

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
  selectedOption: ProjectScanSummary | undefined;

  @Input()
  set selected(scanId: string | null | undefined) {
    this.selectedOption = this.options.find(option => option.scanId == scanId);
  }

  @Input()
  options: ProjectScanSummary[] = [];
  @Output()
  selectChange = new EventEmitter<string>();

  onClick(option: ProjectScanSummary) {
    this.selectedOption = option;
    this.selectChange.emit(option.scanId);
  }

  getIcon(action: GitAction | undefined): string {
    if (action) {
      return this.mIcon.get(action) ?? '';
    }
    return '';
  }
  private mIcon: Map<GitAction, string> = new Map<GitAction, string>([
    [GitAction.CommitTag, 'git-tag'],
    [GitAction.CommitBranch, 'git-branch'],
    [GitAction.MergeRequest, 'git-merge'],
  ]);

  protected readonly GitAction = GitAction;
}
