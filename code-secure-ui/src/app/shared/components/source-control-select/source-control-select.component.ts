import {Component, EventEmitter, input, Output} from '@angular/core';
import {Select} from 'primeng/select';
import {FormsModule} from '@angular/forms';
import {NgIcon} from '@ng-icons/core';
import {LowerCasePipe} from '@angular/common';
import {FloatLabel} from 'primeng/floatlabel';
import {SourceControlSummary} from '../../../api/models/source-control-summary';

@Component({
  selector: 'source-control-select',
  imports: [
    Select,
    FormsModule,
    NgIcon,
    LowerCasePipe,
    FloatLabel,
  ],
  templateUrl: './source-control-select.component.html',
  standalone: true,
  styleUrl: './source-control-select.component.scss'
})
export class SourceControlSelectComponent {
  options = input<SourceControlSummary[]>();
  sourceControlId = input<string | null>();
  @Output()
  onChange = new EventEmitter<string>();
}
