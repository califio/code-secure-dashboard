import {Component, EventEmitter, input, Output} from '@angular/core';
import {Select} from 'primeng/select';
import {SourceControl} from '../../../api/models/source-control';
import {FormsModule} from '@angular/forms';
import {NgIcon} from '@ng-icons/core';
import {LowerCasePipe} from '@angular/common';
import {FindingStatusLabelComponent} from '../finding/finding-status-label/finding-status-label.component';
import {FloatLabel} from 'primeng/floatlabel';
import {MultiSelect} from 'primeng/multiselect';

@Component({
  selector: 'source-control-select',
  imports: [
    Select,
    FormsModule,
    NgIcon,
    LowerCasePipe,
    FindingStatusLabelComponent,
    FloatLabel,
    MultiSelect
  ],
  templateUrl: './source-control-select.component.html',
  standalone: true,
  styleUrl: './source-control-select.component.scss'
})
export class SourceControlSelectComponent {
  options = input<SourceControl[]>();
  sourceControlId = input<string | null>();
  @Output()
  onChange = new EventEmitter<string>();
}
