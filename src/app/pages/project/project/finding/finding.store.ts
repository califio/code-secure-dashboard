import {Injectable, signal} from '@angular/core';
import {ProjectFinding} from '../../../../api/models/project-finding';
import {ProjectFindingFilter} from '../../../../api/models/project-finding-filter';
import {ProjectFindingSortField} from '../../../../api/models/project-finding-sort-field';
import {ProjectScanSummary} from '../../../../api/models/project-scan-summary';

@Injectable({
  providedIn: 'root'
})
export class FindingStore {
  loading = signal(false);
  currentPage = signal(0);
  totalPage = signal(0);
  findings = signal<ProjectFinding[]>([])
  filter: ProjectFindingFilter = {
    desc: true,
    name: '',
    page: 1,
    scanner: null,
    severity: undefined,
    sortBy: ProjectFindingSortField.UpdatedAt,
    status: undefined,
    type: undefined,
    scanId: undefined,
  };
  branches = signal<ProjectScanSummary[]>([]);
  constructor() { }
}
