import {Component, EventEmitter, model, Output} from '@angular/core';
import {FloatLabel} from 'primeng/floatlabel';
import {MultiSelect, MultiSelectChangeEvent} from 'primeng/multiselect';
import {RiskLevel} from '../../../../api/models';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'package-severity-filter',
  imports: [
    FloatLabel,
    MultiSelect,
    FormsModule
  ],
  templateUrl: './package-severity-filter.component.html',
  standalone: true,
})
export class PackageSeverityFilterComponent {
  options = [
    {label: 'Critical', value: RiskLevel.Critical},
    {label: 'High', value: RiskLevel.High},
    {label: 'Medium', value: RiskLevel.Medium},
    {label: 'Low', value: RiskLevel.Low},
    {label: 'None', value: RiskLevel.None},
  ];
  severity = model<RiskLevel[] | null | undefined>([]);
  @Output()
  onChange = new EventEmitter<RiskLevel[]>();

  onChangeSeverity($event: MultiSelectChangeEvent) {
    this.onChange.emit($event.value);
  }
}
