import {Component, EventEmitter, input, Output} from '@angular/core';
import {CommitType} from '../../../api/models/commit-type';
import {transformArrayNotNull} from '../../../core/transform';
import {FloatLabel} from 'primeng/floatlabel';
import {Select} from 'primeng/select';
import {FormsModule} from '@angular/forms';
import {ScanBranchLabelComponent} from '../scan/scan-branch-label/scan-branch-label.component';

export interface BranchOption {
  id: string
  commitType: CommitType,
  commitBranch: string,
  targetBranch?: string
}

@Component({
  selector: 'branch-filter',
  imports: [
    FloatLabel,
    Select,
    FormsModule,
    ScanBranchLabelComponent
  ],
  templateUrl: './branch-filter.component.html',
  styleUrl: './branch-filter.component.scss',
  standalone: true,
})
export class BranchFilterComponent {
  showClear = input(true);
  options = input([], {transform: transformArrayNotNull<BranchOption>});
  value = input<string | null>();
  @Output()
  onChange = new EventEmitter<string>();

  onSelectedChange($event: any) {
    this.onChange.emit($event);
  }

  typedValue(value: any): BranchOption {
    return value;
  }
}
