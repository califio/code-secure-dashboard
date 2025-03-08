import {Component, EventEmitter, input, Output} from '@angular/core';
import {FindingStatusLabelComponent} from '../../finding/finding-status-label/finding-status-label.component';
import {FloatLabel} from 'primeng/floatlabel';
import {MultiSelect} from 'primeng/multiselect';
import {Select, SelectChangeEvent} from 'primeng/select';
import {FormsModule} from '@angular/forms';
import {PackageStatus} from '../../../../api/models/package-status';
import {transformValueNotNull} from '../../../../core/transform';
import {PackageStatusComponent} from '../package-status/package-status.component';

@Component({
  selector: 'package-status-filter',
  imports: [
    FindingStatusLabelComponent,
    FloatLabel,
    MultiSelect,
    Select,
    FormsModule,
    PackageStatusComponent
  ],
  templateUrl: './package-status-filter.component.html',
  standalone: true,
  styleUrl: './package-status-filter.component.scss'
})
export class PackageStatusFilterComponent {
  status = input(PackageStatus.Open, {
    transform: (value: PackageStatus | null | undefined) => transformValueNotNull(value, PackageStatus.Open)
  });
  @Output()
  onChange = new EventEmitter<PackageStatus>();

  options = [
    {
      label: 'Open',
      value: PackageStatus.Open
    },
    {
      label: 'Accepted Risk',
      value: PackageStatus.Ignore
    },
    {
      label: 'Fixed',
      value: PackageStatus.Fixed
    }
  ];

  onChangeStatus($event: SelectChangeEvent) {
    this.onChange.emit($event.value);
  }
}
