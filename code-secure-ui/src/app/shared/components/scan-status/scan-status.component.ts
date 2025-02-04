import {Component, Input} from '@angular/core';
import {ScanStatus} from '../../../api/models/scan-status';
import {NgIcon} from '@ng-icons/core';
import {NgClass} from '@angular/common';

@Component({
  selector: 'scan-status',
  standalone: true,
  imports: [
    NgIcon,
    NgClass
  ],
  templateUrl: './scan-status.component.html',
  styleUrl: './scan-status.component.scss'
})
export class ScanStatusComponent {
  @Input()
  status: ScanStatus = ScanStatus.Queue;
  @Input() showLabel = true;

  getStyle(status: ScanStatus) {
    return this.mStyle.get(status)
  }

  getIcon(status: ScanStatus) {
    return this.mIcon.get(status)
  }

  private mStyle: Map<ScanStatus, string> = new Map<ScanStatus, string>([
    [ScanStatus.Queue, 'text-orange-500'],
    [ScanStatus.Running, 'text-blue-500 animate-spin'],
    [ScanStatus.Completed, 'text-green-500'],
    [ScanStatus.Error, 'text-red-500'],
  ])
  private mIcon: Map<ScanStatus, string> = new Map<ScanStatus, string>([
    [ScanStatus.Queue, 'heroPauseCircle'],
    [ScanStatus.Running, 'spin'],
    [ScanStatus.Completed, 'heroCheckCircle'],
    [ScanStatus.Error, 'heroMinusCircle'],
  ])
}
