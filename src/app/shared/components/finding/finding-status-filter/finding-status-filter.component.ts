import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {FindingStatus} from '../../../../api/models/finding-status';
import {ClickOutsideDirective} from '../../../directives/click-outside.directive';
import {NgClass} from '@angular/common';
import {FindingStatusLabelComponent} from '../finding-status-label/finding-status-label.component';


interface FindingStatusOption {
  status: FindingStatus | undefined
  label: string
  icon: string
  style: string
}


@Component({
  selector: 'finding-status-filter',
  standalone: true,
  imports: [
    NgIcon,
    ClickOutsideDirective,
    NgClass,
    FindingStatusLabelComponent
  ],
  templateUrl: './finding-status-filter.component.html',
  styleUrl: './finding-status-filter.component.scss'
})
export class FindingStatusFilterComponent {
  @Output()
  statusChange = new EventEmitter<FindingStatus | undefined>();

  @Input()
  set status(value: FindingStatus | undefined) {
    this._status = value;
    this.statusOption = this.statusOptions.find(option => option.status == value);
  }

  _status: FindingStatus | undefined = undefined;
  statusOption: FindingStatusOption | undefined = undefined;
  hidden = true;

  constructor() {
  }

  onSelectOption(option: FindingStatusOption) {
    this.statusChange.emit(option.status);
    this.statusOption = option;
    this.hidden = true;
    this._status = option.status;
  }

  statusOptions: FindingStatusOption[] = [
    {
      status: undefined,
      label: 'All',
      icon: 'square',
      style: 'border-b cursor-pointer'
    },
    {
      status: FindingStatus.Open,
      label: 'Open',
      icon: 'open',
      style: 'border-b rounded-t-lg cursor-pointer'
    },
    {
      status: FindingStatus.Confirmed,
      label: 'Confirmed',
      icon: 'verified',
      style: 'border-b text-yellow-500 cursor-pointer'
    },
    {
      status: FindingStatus.Incorrect,
      label: 'False Positive',
      icon: 'dislike',
      style: 'border-b cursor-pointer'
    },
    {
      status: FindingStatus.Ignore,
      label: 'Accepted Risk',
      icon: 'warning',
      style: 'border-b text-orange-500 cursor-pointer'
    },
    {
      status: FindingStatus.Fixed,
      label: 'Fixed',
      icon: 'verified',
      style: 'rounded-b-lg text-green-500 cursor-pointer'
    }
  ];
}

