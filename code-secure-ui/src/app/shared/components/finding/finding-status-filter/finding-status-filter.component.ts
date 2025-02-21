import {Component, EventEmitter, input, Output} from '@angular/core';
import {FindingStatusLabelComponent} from '../finding-status-label/finding-status-label.component';
import {FloatLabel} from 'primeng/floatlabel';
import {MultiSelect, MultiSelectChangeEvent} from 'primeng/multiselect';
import {FindingStatus} from '../../../../api/models/finding-status';
import {getFindingStatusOptions} from '../finding-status';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'finding-status-filter',
  imports: [
    FindingStatusLabelComponent,
    FloatLabel,
    MultiSelect,
    FormsModule
  ],
  templateUrl: './finding-status-filter.component.html',
  standalone: true,
})
export class FindingStatusFilterComponent {
  status = input<FindingStatus[] | undefined | null>();
  @Output()
  statusChange = new EventEmitter<FindingStatus[]>();
  options = getFindingStatusOptions();

  onChangeStatus($event: MultiSelectChangeEvent) {
    this.statusChange.emit($event.value);
  }
}
