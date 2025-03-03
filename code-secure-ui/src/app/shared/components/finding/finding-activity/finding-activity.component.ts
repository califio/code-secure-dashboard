import {Component, Input, ViewEncapsulation} from '@angular/core';
import {FindingActivity} from '../../../../api/models/finding-activity';
import {FindingStatusLabelComponent} from '../finding-status-label/finding-status-label.component';
import {ScanBranchLabelComponent} from '../../scan/scan-branch-label/scan-branch-label.component';
import {TimeagoModule} from 'ngx-timeago';
import {FindingActivityType} from '../../../../api/models/finding-activity-type';
import {FindingStatus} from '../../../../api/models/finding-status';
import {DatePipe, NgClass, UpperCasePipe} from '@angular/common';
import {Avatar} from 'primeng/avatar';
import {FirstCharPipe} from '../../../pipes/firstchar.pipe';
import {Timeline} from 'primeng/timeline';
import {Panel} from 'primeng/panel';
import {Tooltip} from 'primeng/tooltip';
import {MarkdownComponent} from 'ngx-markdown';
import {CommitType} from '../../../../api/models';

@Component({
  selector: 'finding-activity',
  standalone: true,
  imports: [
    FindingStatusLabelComponent,
    ScanBranchLabelComponent,
    TimeagoModule,
    DatePipe,
    Avatar,
    FirstCharPipe,
    Timeline,
    UpperCasePipe,
    Panel,
    NgClass,
    Tooltip,
    MarkdownComponent
  ],
  templateUrl: './finding-activity.component.html',
  styleUrl: './finding-activity.component.scss',
  encapsulation: ViewEncapsulation.None
})
export class FindingActivityComponent {
  @Input()
  activities: FindingActivity[] = [];

  parseDate(text: string | null | undefined) {
    if (text) {
      return new Date(text);
    }
    return null;
  }

  activityIcon(activityType: FindingActivityType): string {
    if (activityType == FindingActivityType.Open) {
      return 'pi pi-map-marker';
    }
    if (activityType == FindingActivityType.Reopen) {
      return 'pi pi-map-marker';
    }
    if (activityType == FindingActivityType.Fixed) {
      return 'pi-check-circle';
    }
    if (activityType == FindingActivityType.Comment) {
      return 'pi pi-comment';
    }
    if (activityType == FindingActivityType.ChangeStatus) {
      return 'pi pi-replay';
    }
    if (activityType == FindingActivityType.ChangeDeadline) {
      return 'pi pi-history';
    }
    return 'pi pi-circle';
  }

  protected readonly FindingActivityType = FindingActivityType;
  protected readonly FindingStatus = FindingStatus;
  protected readonly CommitType = CommitType;
}
