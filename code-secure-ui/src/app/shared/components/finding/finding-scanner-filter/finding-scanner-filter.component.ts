import {Component, EventEmitter, input, model, Output} from '@angular/core';
import {FloatLabel} from 'primeng/floatlabel';
import {MultiSelect, MultiSelectChangeEvent} from 'primeng/multiselect';
import {ScannerLabelComponent} from '../../scanner-label/scanner-label.component';
import {FormsModule} from '@angular/forms';
import {Scanners} from '../../../../api/models/scanners';

@Component({
  selector: 'finding-scanner-filter',
  imports: [
    FloatLabel,
    MultiSelect,
    ScannerLabelComponent,
    FormsModule
  ],
  templateUrl: './finding-scanner-filter.component.html',
  standalone: true,
  styleUrl: './finding-scanner-filter.component.scss'
})
export class FindingScannerFilterComponent {

  scanners = input<Scanners[]>();
  selected = model<string[] |null>();
  @Output()
  onChange = new EventEmitter<string[]>();

  onChangeScanners($event: MultiSelectChangeEvent) {
    this.onChange.emit($event.value);
  }
}
