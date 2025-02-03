import {Injectable, signal} from '@angular/core';
import {ProjectFinding} from '../../../../api/models/project-finding';
import {ProjectFindingFilter} from '../../../../api/models/project-finding-filter';
import {ProjectFindingSortField} from '../../../../api/models/project-finding-sort-field';
import {ProjectCommitSummary} from '../../../../api/models/project-commit-summary';
import {ProjectScanner} from '../../../../api/models/project-scanner';
import {FindingStatus} from '../../../../api/models/finding-status';
import {DropdownItem} from '../../../../shared/ui/dropdown/dropdown.model';

@Injectable({
  providedIn: 'root'
})
export class FindingStore {
  loading = signal(false);
  currentPage = signal(0);
  totalPage = signal(0);
  commits = signal<DropdownItem[]>([]);
  //branch = signal<DropdownItem[]>([]);
  scanners = signal<ProjectScanner[]>([]);
  findings = signal<ProjectFinding[]>([])
  filter: ProjectFindingFilter = {
    desc: true,
    name: '',
    page: 1,
    scanner: undefined,
    severity: undefined,
    sortBy: ProjectFindingSortField.CreatedAt,
    status: [],
    type: undefined,
    commitId: undefined,
  };
  constructor() { }
}
