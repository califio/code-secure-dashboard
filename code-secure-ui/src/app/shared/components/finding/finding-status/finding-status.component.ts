import {Component, effect, input, signal} from '@angular/core';
import {Tag} from 'primeng/tag';
import {NgClass} from '@angular/common';
import {FindingStatus} from '../../../../api/models/finding-status';

@Component({
  selector: 'finding-status',
  imports: [
    Tag,
    NgClass
  ],
  templateUrl: './finding-status.component.html',
  standalone: true,
})
export class FindingStatusComponent {
  status = input<FindingStatus>(FindingStatus.Open);
  styleClass = input<string>();
  severity = signal<"success" | "secondary" | "info" | "warn" | undefined>('secondary');
  icon = signal('pi pi-circle');
  label = signal('');

  constructor() {
    effect(() => {
      const statusOption = this.mStatus.get(this.status());
      if (statusOption) {
        this.severity.set(statusOption.severity);
        this.icon.set(statusOption.icon);
        this.label.set(statusOption.label);
      }
    })
  }

  private mStatus = new Map<FindingStatus, any>([
    [
      FindingStatus.Open,
      {
        label: 'Need Triage',
        status: FindingStatus.Open,
        icon: 'pi pi-circle',
        severity: 'secondary'
      }
    ],
    [
      FindingStatus.Confirmed,
      {
        label: 'Confirmed',
        status: FindingStatus.Confirmed,
        icon: 'pi pi-verified',
        severity: 'info'
      }
    ],
    [
      FindingStatus.AcceptedRisk,
      {
        label: 'Accepted Risk',
        status: FindingStatus.AcceptedRisk,
        icon: 'pi pi-exclamation-triangle',
        severity: 'warn'
      }
    ],
    [
      FindingStatus.Fixed,
      {
        label: 'Fixed',
        status: FindingStatus.Fixed,
        icon: 'pi pi-verified',
        severity: 'success'
      }
    ],
    [
      FindingStatus.Incorrect,
      {
        label: 'False Positive',
        status: FindingStatus.Incorrect,
        icon: 'pi pi-thumbs-down',
        severity: 'secondary'
      }
    ]
  ]);
}
