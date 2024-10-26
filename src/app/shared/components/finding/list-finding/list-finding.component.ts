import {Component, EventEmitter, HostListener, input, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {DropdownComponent} from '../../dropdown/dropdown.component';
import {DropdownItem} from '../../dropdown/dropdown.model';
import {TimeagoModule} from 'ngx-timeago';
import {Router, RouterLink} from '@angular/router';
import {PaginationComponent} from '../../pagination/pagination.component';
import {LoadingTableComponent} from '../../loading-table/loading-table.component';
import {StatusFindingComponent} from '../../status-finding/status-finding.component';
import {Finding} from '../finding.model';
import {StatusFinding} from '../../status-finding/status-finding.model';
import {NgClass} from '@angular/common';

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
    StatusFindingComponent,
    NgClass
  ],
  templateUrl: './list-finding.component.html',
  styleUrl: './list-finding.component.scss'
})
export class ListFindingComponent {
  loading = input<boolean>(true);
  findings = input<Finding[]>([]);
  @Output()
  openFinding = new EventEmitter<Finding>();
  search = '';
  exportOptions: DropdownItem[] = [
    {
      value: 'csv',
      label: 'CSV'
    },
    {
      value: 'json',
      label: 'JSON'
    },
    {
      value: 'pdf',
      label: 'PDF'
    },
  ];

  onSearchChange() {

  }

  onOpenFinding(finding: Finding) {
    this.openFinding.emit(finding);
  }
  mStatus: Map<StatusFinding, any> = new Map<StatusFinding, any>([
    ['open', {'label': 'Open', 'icon': 'open', 'style': ''}],
    ['confirmed', {'label': 'Confirmed', 'icon': 'verified', 'style': 'text-yellow-500'}],
    ['ignore', {'label': 'Accepted Risk', 'icon': 'warning', 'style': 'text-orange-500'}],
    ['false_positive', {'label': 'False Positive', 'icon': 'dislike', 'style': ''}],
    ['fixed', {'label': 'Fixed', 'icon': 'verified', 'style': 'text-green-500'}],
  ]);
}
