import {Component, Input} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {GitAction} from '../../../api/models/git-action';
import {getGitActionIcon} from '../../helper';

@Component({
  selector: 'scan-branch',
  standalone: true,
    imports: [
        NgIcon
    ],
  templateUrl: './scan-branch.component.html',
  styleUrl: './scan-branch.component.scss'
})
export class ScanBranchComponent {
  @Input()
  action: GitAction = GitAction.CommitBranch;
  @Input()
  branch = '';
  @Input()
  targetBranch: string | undefined | null = null;
  protected readonly GitAction = GitAction;
  protected readonly getGitActionIcon = getGitActionIcon;
}
