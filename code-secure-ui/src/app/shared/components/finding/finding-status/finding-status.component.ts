import {Component, EventEmitter, Input, input, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {NgClass} from '@angular/common';
import {FindingStatusOption} from './status-finding.model';
import {FindingStatus} from '../../../../api/models/finding-status';
import {ClickOutsideDirective} from '../../../directives/click-outside.directive';

@Component({
  selector: 'finding-status',
  standalone: true,
  imports: [
    NgIcon,
    NgClass,
    ClickOutsideDirective
  ],
  templateUrl: './finding-status.component.html',
  styleUrl: './finding-status.component.scss'
})
export class FindingStatusComponent {
  @Output()
  statusChange = new EventEmitter<FindingStatus>();
  @Input()
  set status(value: FindingStatus) {
    this._status = value;
    this.statusOption = this.statusOptions.find(option => option.status == value);
  }
  _status: FindingStatus = FindingStatus.Open;
  fixedLabel = input<string>()
  showDescription = input(true);
  statusOption: FindingStatusOption | undefined = undefined;
  hidden = true;
  constructor() {
  }

  onSelectOption(option: FindingStatusOption) {
    if (option.status != FindingStatus.Fixed) {
      this.statusChange.emit(option.status);
      this.statusOption = option;
      this.hidden = true;
      this._status = option.status;
    }
  }

  statusOptions: FindingStatusOption[] = [
    {
      status: FindingStatus.Open,
      label: 'Open',
      description: 'Newly discovered vulnerabilities. Will remain open until reviewed or retested.',
      icon: 'open',
      style: 'border-b rounded-t-lg cursor-pointer'
    },
    {
      status: FindingStatus.Confirmed,
      label: 'Confirmed',
      description: 'Vulnerabilities that have been acknowledged as valid and require remediation.',
      icon: 'verified',
      style: 'border-b text-yellow-500 cursor-pointer'
    },
    {
      status: FindingStatus.Incorrect,
      label: 'False Positive',
      description: 'Vulnerabilities incorrectly identified. Marking as false positive to exclude them from future scans.',
      icon: 'dislike',
      style: 'border-b cursor-pointer'
    },
    {
      status: FindingStatus.AcceptedRisk,
      label: 'Accepted Risk',
      description: 'Vulnerabilities acknowledged but deemed low risk. Accepted without a fix and excluded from future scans.',
      icon: 'warning',
      style: 'border-b text-orange-500 cursor-pointer'
    },
    {
      status: FindingStatus.Fixed,
      label: 'Fixed',
      description: 'Vulnerabilities that are no longer detected. Status is updated automatically from future scans.',
      icon: 'verified',
      style: 'rounded-b-lg text-green-500 cursor-not-allowed'
    },
  ]
}
