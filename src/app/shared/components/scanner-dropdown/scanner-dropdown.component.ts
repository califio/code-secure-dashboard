import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {ClickOutsideDirective} from '../../directives/click-outside.directive';
import {ProjectScanner} from '../../../api/models/project-scanner';
import {ScannerLabelComponent} from '../scanner-label/scanner-label.component';
import {NgClass} from '@angular/common';

@Component({
  selector: 'scanner-dropdown',
  standalone: true,
  imports: [
    NgIcon,
    ClickOutsideDirective,
    ScannerLabelComponent,
    NgClass
  ],
  templateUrl: './scanner-dropdown.component.html',
  styleUrl: './scanner-dropdown.component.scss'
})
export class ScannerDropdownComponent {
  hidden = true;
  label = 'Scanner';
  scannerSelected: ProjectScanner | undefined | null;

  @Input()
  set scanner(scanner: ProjectScanner | null | undefined) {
    this.scannerSelected = this.options.find(option => option.name == scanner?.name && option.type == scanner?.type);
  }
  @Input()
  options: ProjectScanner[] = [];
  @Output()
  scannerChange = new EventEmitter<ProjectScanner | null>();

  onClick(option: ProjectScanner | null) {
    this.scannerSelected = option;
    this.scannerChange.emit(option);
  }
}
