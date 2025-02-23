import {Component, Input} from '@angular/core';
import {ScanStatus} from '../../../../api/models/scan-status';
import {NgClass} from '@angular/common';

@Component({
  selector: 'scan-status',
  standalone: true,
  imports: [
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

  private mStyle: Map<ScanStatus, string> = new Map<ScanStatus, string>([
    [ScanStatus.Queue, 'pi pi-pause-circle text-orange-500'],
    [ScanStatus.Running, 'pi pi-spin pi-spinner text-blue-500'],
    [ScanStatus.Completed, 'pi pi-check-circle text-green-500'],
    [ScanStatus.Error, 'pi pi-minus-circle text-red-500'],
  ])
}
