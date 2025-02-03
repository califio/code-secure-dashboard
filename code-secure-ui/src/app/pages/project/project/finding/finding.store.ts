import {Injectable, signal} from '@angular/core';
import {ProjectFinding} from '../../../../api/models/project-finding';
import {ProjectFindingFilter} from '../../../../api/models/project-finding-filter';
import {ProjectFindingSortField} from '../../../../api/models/project-finding-sort-field';
import {DropdownItem} from '../../../../shared/ui/dropdown/dropdown.model';

@Injectable({
  providedIn: 'root'
})
export class FindingStore {
  loading = signal(false);
  currentPage = signal(0);
  totalPage = signal(0);
  commits = signal<DropdownItem[]>([]);
  scanners = signal<DropdownItem[]>([]);
  findings = signal<ProjectFinding[]>([])
  filter: ProjectFindingFilter = {
    desc: true,
    name: '',
    page: 1,
    scanner: [],
    severity: undefined,
    sortBy: ProjectFindingSortField.CreatedAt,
    status: [],
    commitId: undefined,
  };

  constructor() {
  }
}
