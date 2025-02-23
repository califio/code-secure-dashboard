import {computed, Injectable, signal} from '@angular/core';
import {ProjectCommitSummary} from '../../../../api/models/project-commit-summary';
import {Scanners} from '../../../../api/models/scanners';
import {FindingDetail} from '../../../../api/models/finding-detail';
import {FindingSortField, FindingSummary, ProjectFindingFilter} from '../../../../api/models';

@Injectable({
  providedIn: 'root'
})
export class FindingStore {
  commits = signal<ProjectCommitSummary[]>([]);
  scanners = signal<Scanners[]>([]);
  rules = signal<string[]>([]);
  // list findings
  loading = signal(false);
  findings = signal<FindingSummary[]>([]);
  // finding detail
  showFinding = signal(false);
  loadingFinding = signal(false);
  finding = signal<FindingDetail | null>(null);
  loadingExport = signal(false);
  // filter
  filter: ProjectFindingFilter = {};
  //paginator
  currentPage = signal(1);
  pageSize = signal(20);
  totalRecords = signal(0);
  firstRecord = computed(() => {
    return (this.currentPage() - 1) * this.pageSize();
  })
  //
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
