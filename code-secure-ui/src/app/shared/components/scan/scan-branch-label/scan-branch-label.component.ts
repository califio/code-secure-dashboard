import {Component, input, Input} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {getGitActionIcon} from '../../../helper';
import {CommitType} from '../../../../api/models/commit-type';
import {transformStringNotNull, transformValueNotNull} from '../../../../core/transform';

@Component({
  selector: 'scan-branch-label',
  standalone: true,
  imports: [
    NgIcon
  ],
  templateUrl: './scan-branch-label.component.html',
})
export class ScanBranchLabelComponent {
  commitType = input(CommitType.CommitBranch, {
    transform: (value: CommitType | null | undefined) => transformValueNotNull(value, CommitType.CommitBranch)
  });
  commitBranch = input('', {transform: transformStringNotNull});
  targetBranch = input<string | null | undefined>(null);
  protected readonly getGitActionIcon = getGitActionIcon;
  protected readonly CommitType = CommitType;
}

