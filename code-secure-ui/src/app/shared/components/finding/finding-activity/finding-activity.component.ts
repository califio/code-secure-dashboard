import {Component, Input} from '@angular/core';
import {FindingActivity} from '../../../../api/models/finding-activity';
import {AvatarComponent} from '../../../ui/avatar/avatar.component';
import {FindingStatusLabelComponent} from '../finding-status-label/finding-status-label.component';
import {NgIcon} from '@ng-icons/core';
import {ScanBranchComponent} from '../../scan-branch/scan-branch.component';
import {TimeagoModule} from 'ngx-timeago';
import {FindingActivityType} from '../../../../api/models/finding-activity-type';
import {GitAction} from '../../../../api/models/git-action';
import {FindingStatus} from '../../../../api/models/finding-status';
import {TooltipDirective} from '../../../ui/tooltip/tooltip.directive';
import {DatePipe} from '@angular/common';

@Component({
  selector: 'finding-activity',
  standalone: true,
  imports: [
    AvatarComponent,
    FindingStatusLabelComponent,
    NgIcon,
    ScanBranchComponent,
    TimeagoModule,
    TooltipDirective,
    DatePipe
  ],
  templateUrl: './finding-activity.component.html',
  styleUrl: './finding-activity.component.scss'
})
export class FindingActivityComponent {
  @Input()
  activity: FindingActivity = {};

  parseDate(text: string | null | undefined) {
    if (text) {
      return new Date(text);
    }
    return null;
  }
  protected readonly FindingActivityType = FindingActivityType;
  protected readonly GitAction = GitAction;
  protected readonly FindingStatus = FindingStatus;
}
