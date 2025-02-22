import {Component, EventEmitter, model, Output} from '@angular/core';
import {FloatLabel} from 'primeng/floatlabel';
import {MultiSelect, MultiSelectChangeEvent} from 'primeng/multiselect';
import {FormsModule} from '@angular/forms';
import {FindingSeverity} from '../../../../api/models/finding-severity';

@Component({
  selector: 'finding-severity-filter',
  imports: [
    FloatLabel,
    MultiSelect,
    FormsModule
  ],
  templateUrl: './finding-severity-filter.component.html',
  standalone: true,
})
export class FindingSeverityFilterComponent {
  options = [
    {label: 'Critical', value: FindingSeverity.Critical},
    {label: 'High', value: FindingSeverity.High},
    {label: 'Medium', value: FindingSeverity.Medium},
    {label: 'Low', value: FindingSeverity.Low},
    {label: 'Info', value: FindingSeverity.Info},
  ];
  severity = model<FindingSeverity[] | null | undefined>([]);
  @Output()
  onChange = new EventEmitter<FindingSeverity[]>();

  onChangeSeverity($event: MultiSelectChangeEvent) {
    this.onChange.emit($event.value);
  }
}
