import {Injectable, signal} from '@angular/core';
import {ProjectFinding} from '../../../../api/models/project-finding';
import {ProjectFindingFilter} from '../../../../api/models/project-finding-filter';
import {ProjectFindingSortField} from '../../../../api/models/project-finding-sort-field';
import {ProjectCommitSummary} from '../../../../api/models/project-commit-summary';
import {ProjectScanner} from '../../../../api/models/project-scanner';

@Injectable({
  providedIn: 'root'
})
export class FindingStore {
  loading = signal(false);
  currentPage = signal(0);
  totalPage = signal(0);
  commits = signal<ProjectCommitSummary[]>([]);
  scanners = signal<ProjectScanner[]>([]);
  findings = signal<ProjectFinding[]>([])
  filter: ProjectFindingFilter = {
    desc: true,
    name: '',
    page: 1,
    scanner: undefined,
    severity: undefined,
    sortBy: ProjectFindingSortField.UpdatedAt,
    status: undefined,
    type: undefined,
    commitId: undefined,
  };
  constructor() { }
}
