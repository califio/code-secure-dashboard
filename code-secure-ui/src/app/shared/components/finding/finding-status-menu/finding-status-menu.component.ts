import {Component, effect, EventEmitter, input, Output, signal} from '@angular/core';
import {transformValueNotNull} from '../../../../core/transform';
import {FindingStatus} from '../../../../api/models';
import {NgClass} from '@angular/common';
import {Menu} from 'primeng/menu';
import {FindingStatusComponent} from '../finding-status/finding-status.component';

@Component({
  selector: 'finding-status-menu',
  imports: [
    NgClass,
    Menu,
    FindingStatusComponent
  ],
  templateUrl: './finding-status-menu.component.html',
  standalone: true,
})
export class FindingStatusMenuComponent {
  styleClass = input('')
  status = input(FindingStatus.Open, {
    transform: (value: FindingStatus | null | undefined) => transformValueNotNull(value, FindingStatus.Open)
  });
  @Output()
  onChange = new EventEmitter<FindingStatus>();
  statusOptions: any[] = [
    FindingStatus.Open,
    FindingStatus.Confirmed,
    FindingStatus.AcceptedRisk,
    FindingStatus.Fixed,
    FindingStatus.Incorrect,
  ];
  selectedOption = signal<FindingStatus>(this.statusOptions[0]);

  constructor() {
    effect(() => {
      this.selectedOption.set(this.status());
    });
  }

  onChangeStatus(status: FindingStatus) {
    if (status != this.status()) {
      this.selectedOption.set(status);
      this.onChange.emit(status);
    }
  }

}
