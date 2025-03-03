import {Component, Input} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {getGitActionIcon} from '../../../helper';
import {CommitType} from '../../../../api/models/commit-type';

@Component({
  selector: 'scan-branch-label',
  standalone: true,
  imports: [
    NgIcon
  ],
  templateUrl: './scan-branch-label.component.html',
})
export class ScanBranchLabelComponent {
  @Input()
  action: CommitType = CommitType.CommitBranch;
  @Input()
  branch = '';
  @Input()
  targetBranch: string | undefined | null = null;
  protected readonly getGitActionIcon = getGitActionIcon;
  protected readonly CommitType = CommitType;
}
