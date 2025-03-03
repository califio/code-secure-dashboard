import {Component, Input} from '@angular/core';
import {ScannerType} from '../../../api/models/scanner-type';

@Component({
  selector: 'scanner-label',
  standalone: true,
  templateUrl: './scanner-label.component.html',
})
export class ScannerLabelComponent {
  @Input()
  scanner: string | undefined | null = '';
  @Input()
  type: ScannerType | undefined | null;
}
