import {Component, EventEmitter, input, Output} from '@angular/core';
import {getFindingStatusOptions} from '../finding-status';
import {Select} from 'primeng/select';
import {FormsModule} from '@angular/forms';
import {FindingStatusLabelComponent} from '../finding-status-label/finding-status-label.component';
import {FindingStatus} from '../../../../api/models/finding-status';

@Component({
  selector: 'finding-status-select',
  imports: [
    Select,
    FormsModule,
    FindingStatusLabelComponent
  ],
  templateUrl: './finding-status-select.component.html',
  standalone: true,
})
export class FindingStatusSelectComponent {
  status = input<FindingStatus | null>();
  @Output()
  onChange = new EventEmitter<FindingStatus>();
  statusOptions = getFindingStatusOptions();
  onChangeStatus($event: any) {
    this.onChange.emit($event);
  }
}
