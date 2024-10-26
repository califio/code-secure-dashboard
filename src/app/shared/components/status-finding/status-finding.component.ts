import {Component, computed, effect, EffectRef, EventEmitter, input, OnDestroy, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {NgClass} from '@angular/common';
import {StatusFinding, StatusFindingOption} from './status-finding.model';

@Component({
  selector: 'status-finding',
  standalone: true,
  imports: [
    NgIcon,
    NgClass
  ],
  templateUrl: './status-finding.component.html',
  styleUrl: './status-finding.component.scss'
})
export class StatusFindingComponent implements OnDestroy {
  @Output()
  statusChange = new EventEmitter<StatusFinding>();
  status = input<StatusFinding | undefined>();
  fixedLabel = input<string>()
  statusOption: StatusFindingOption | undefined = undefined;
  hidden = true;
  constructor() {
    this.destroy$ = effect(() => {
      const status = this.status()
      this.statusOption = this.statusOptions.find(option => option.status == status);
    })
  }

  onSelectOption(option: StatusFindingOption) {
    this.statusChange.emit(option.status);
    this.statusOption = option;
    this.hidden = true;
  }

  ngOnDestroy(): void {
    this.destroy$.destroy();
  }

  private destroy$: EffectRef;
  statusOptions: StatusFindingOption[] = [
    {
      status: 'open',
      label: 'Open',
      description: 'Newly discovered vulnerabilities. Will remain open until reviewed or retested.',
      icon: 'open',
      style: 'border-b rounded-t-lg'
    },
    {
      status: 'confirmed',
      label: 'Confirmed',
      description: 'Vulnerabilities that have been acknowledged as valid and require remediation.',
      icon: 'verified',
      style: 'border-b text-yellow-500'
    },
    {
      status: 'false_positive',
      label: 'False Positive',
      description: 'Vulnerabilities incorrectly identified. Marking as false positive to exclude them from future scans.',
      icon: 'dislike',
      style: 'border-b'
    },
    {
      status: 'ignore',
      label: 'Accepted Risk',
      description: 'Vulnerabilities acknowledged but deemed low risk. Accepted without a fix and excluded from future scans.',
      icon: 'warning',
      style: 'border-b text-orange-500'
    },
    {
      status: 'fixed',
      label: 'Fixed',
      description: 'Vulnerabilities that are no longer detected. Status is updated automatically after a manual or automatic retest.',
      icon: 'verified',
      style: 'rounded-b-lg text-green-500'
    },
  ]
}
