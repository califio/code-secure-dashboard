import {Component, Input} from '@angular/core';
import {ScannerType} from '../../../api/models/scanner-type';
import {NgIcon} from '@ng-icons/core';

@Component({
  selector: 'scanner-label',
  standalone: true,
  imports: [
    NgIcon
  ],
  templateUrl: './scanner-label.component.html',
  styleUrl: './scanner-label.component.scss'
})
export class ScannerLabelComponent {
  @Input()
  scanner: string | undefined | null = '';
  @Input()
  type: ScannerType | undefined | null = ScannerType.Sast;
}
