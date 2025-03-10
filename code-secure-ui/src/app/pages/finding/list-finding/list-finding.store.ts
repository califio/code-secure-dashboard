import {computed, Injectable, signal} from '@angular/core';
import {Scanners} from '../../../api/models/scanners';
import {FindingDetail} from '../../../api/models/finding-detail';
import {FindingSummary} from '../../../api/models/finding-summary';
import {FindingFilter} from '../../../api/models/finding-filter';
import {FindingSortField, FindingStatus, SourceControls, UserSummary} from '../../../api/models';
import {getFindingStatusOptions} from '../../../shared/components/finding/finding-status';
import {RangeDateState} from '../../../shared/ui/range-date/range-date.model';

@Injectable({
  providedIn: 'root'
})
export class ListFindingStore {
  users = signal<UserSummary[]>([]);
  scanners = signal<Scanners[]>([]);
  rules = signal<string[]>([]);
  categories = signal<string[]>([]);
  sourceControls = signal<SourceControls[]>([]);
  // list findings
  loading = signal(false);
  findings = signal<FindingSummary[]>([]);
  // finding detail
  showFinding = signal(false);
  loadingFinding = signal(false);
  finding = signal<FindingDetail | null>(null);
  createdAtRangeDate = signal<RangeDateState>({});
  fixedAtRangeDate = signal<RangeDateState>({});
  //paginator
  currentPage = signal(1);
  pageSize = signal(20);
  totalRecords = signal(0);
  firstRecord = computed(() => {
    return (this.currentPage() - 1) * this.pageSize();
  });
  //
  statusOptions = getFindingStatusOptions();
  sortOptions = [
    {
      value: FindingSortField.Name,
      label: 'name'
    },
    {
      value: FindingSortField.UpdatedAt,
      label: 'updated'
    },
    {
      value: FindingSortField.CreatedAt,
      label: 'created'
    },
    {
      value: FindingSortField.Status,
      label: 'status'
    },
    {
      value: FindingSortField.Severity,
      label: 'severity'
    }
  ];

  constructor() {
  }
}
