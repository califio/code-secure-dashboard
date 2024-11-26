import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {DropdownComponent} from '../../../ui/dropdown/dropdown.component';
import {TimeagoModule} from 'ngx-timeago';
import {RouterLink} from '@angular/router';
import {PaginationComponent} from '../../../ui/pagination/pagination.component';
import {LoadingTableComponent} from '../../../ui/loading-table/loading-table.component';
import {FindingStatusComponent} from '../finding-status/finding-status.component';
import {LowerCasePipe, NgClass} from '@angular/common';
import {ProjectFinding} from '../../../../api/models/project-finding';
import {FindingStatus} from '../../../../api/models/finding-status';
import {FindingSeverity} from '../../../../api/models/finding-severity';
import {ScanBranchDropdownComponent} from '../../scan-branch-dropdown/scan-branch-dropdown.component';
import {FindingStatusLabelComponent} from '../finding-status-label/finding-status-label.component';
import {FindingSeverityComponent} from '../finding-severity/finding-severity.component';

@Component({
  selector: 'list-finding',
  standalone: true,
  imports: [
    NgIcon,
    ReactiveFormsModule,
    FormsModule,
    DropdownComponent,
    TimeagoModule,
    RouterLink,
    PaginationComponent,
    LoadingTableComponent,
    FindingStatusComponent,
    NgClass,
    LowerCasePipe,
    ScanBranchDropdownComponent,
    FindingStatusLabelComponent,
    FindingSeverityComponent
  ],
  templateUrl: './list-finding.component.html',
  styleUrl: './list-finding.component.scss'
})
export class ListFindingComponent {
  @Input()
  loading = false;
  @Input()
  findings: ProjectFinding[] = [];
  @Output()
  openFinding = new EventEmitter<string>();
  @Output()
  selectFindings = new EventEmitter<string[]>();
  selectedFindings: string[] = [];
  constructor() {}

  onOpenFinding(findingId?: string) {
    this.openFinding.emit(findingId);
  }

  onSelectFinding(findingId: string, event: any) {
    if (event.target.checked) {
      if (!this.selectedFindings.find(value => value == findingId)) {
        this.selectedFindings.push(findingId);
        this.selectFindings.emit(this.selectedFindings);
      }
    } else {
      this.selectedFindings = this.selectedFindings.filter(value => value != findingId);
      this.selectFindings.emit(this.selectedFindings);
    }
  }
}
