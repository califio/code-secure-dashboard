import {Component, EventEmitter, Input, Output} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {TimeagoModule} from 'ngx-timeago';
import {ProjectFinding} from '../../../../api/models/project-finding';
import {FindingStatusLabelComponent} from '../finding-status-label/finding-status-label.component';
import {FindingSeverityComponent} from '../finding-severity/finding-severity.component';
import {TableModule} from 'primeng/table';
import {Checkbox, CheckboxChangeEvent} from 'primeng/checkbox';
import {GitAction} from '../../../../api/models/git-action';
import {ScanStatus} from '../../../../api/models/scan-status';

@Component({
  selector: 'list-finding',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    TimeagoModule,
    FindingStatusLabelComponent,
    FindingSeverityComponent,
    TableModule,
    Checkbox,
  ],
  templateUrl: './list-finding.component.html',
})
export class ListFindingComponent {
  @Input()
  loading = false;
  @Input()
  findings: ProjectFinding[] = [];
  @Output()
  onOpenFinding = new EventEmitter<string>();
  @Output()
  onSelectedChange = new EventEmitter<string[]>();

  private _selectedFindings = new Set<string>();

  constructor() {
  }

  emitOpenFinding(findingId?: string) {
    this.onOpenFinding.emit(findingId);
  }

  onSelectFinding(findingId: string, $event: CheckboxChangeEvent) {
    if ($event.checked) {
      this._selectedFindings.add(findingId);
    } else {
      this._selectedFindings.delete(findingId);
    }
    this.onSelectedChange.emit(Array.from(this._selectedFindings.values()));
  }

  protected readonly GitAction = GitAction;
  protected readonly ScanStatus = ScanStatus;
}
