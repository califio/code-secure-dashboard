import {computed, Injectable, signal} from '@angular/core';
import {Scanners} from '../../../../api/models/scanners';
import {FindingDetail} from '../../../../api/models/finding-detail';
import {
  FindingFilter,
  FindingSortField,
  FindingStatus,
  FindingSummary,
} from '../../../../api/models';
import {BranchOption} from '../../../../shared/components/branch-filter/branch-filter.component';

@Injectable({
  providedIn: 'root'
})
export class FindingStore {
  // list findings
  loading = signal(false);
  findings = signal<FindingSummary[]>([]);
  // finding detail
  showFinding = signal(false);
  loadingFinding = signal(false);
  finding = signal<FindingDetail | null>(null);
  loadingExport = signal(false);
  // filter
  filter: FindingFilter = {
    desc: true,
    name: '',
    page: 1,
    scanner: [],
    severity: undefined,
    sortBy: FindingSortField.CreatedAt,
    ruleId: undefined,
    status: [
      FindingStatus.Open,
      FindingStatus.Confirmed,
    ],
    commitId: undefined,
    size: 20,
  };
  //paginator
  currentPage = signal(1);
  pageSize = signal(20);
  totalRecords = signal(0);
  firstRecord = computed(() => {
    return (this.currentPage() - 1) * this.pageSize();
  })
  // filter options
  branchOptions = signal<BranchOption[]>([]);
  scannerOptions = signal<Scanners[]>([]);
  ruleOptions = signal<string[]>([]);
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
