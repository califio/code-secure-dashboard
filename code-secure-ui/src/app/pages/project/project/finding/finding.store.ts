import {computed, Injectable, signal} from '@angular/core';
import {ProjectFinding} from '../../../../api/models/project-finding';
import {ProjectFindingFilter} from '../../../../api/models/project-finding-filter';
import {ProjectFindingSortField} from '../../../../api/models/project-finding-sort-field';
import {ProjectCommitSummary} from '../../../../api/models/project-commit-summary';
import {Scanners} from '../../../../api/models/scanners';
import {FindingStatus} from '../../../../api/models/finding-status';
import {FindingDetail} from '../../../../api/models/finding-detail';

@Injectable({
  providedIn: 'root'
})
export class FindingStore {
  commits = signal<ProjectCommitSummary[]>([]);
  scanners = signal<Scanners[]>([]);
  // list findings
  loadingFindings = signal(false);
  findings = signal<ProjectFinding[]>([]);
  // finding detail
  showFinding = signal(false);
  loadingFinding = signal(false);
  finding = signal<FindingDetail | null>(null);
  // filter
  filter: ProjectFindingFilter = {
    desc: true,
    name: '',
    page: 1,
    scanner: [],
    severity: undefined,
    sortBy: ProjectFindingSortField.CreatedAt,
    status: [
      FindingStatus.Open,
      FindingStatus.Confirmed,
      FindingStatus.Fixed,
      FindingStatus.AcceptedRisk
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

  constructor() {
  }
}
