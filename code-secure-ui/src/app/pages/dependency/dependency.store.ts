import {computed, Injectable, signal} from '@angular/core';
import {ProjectPackage} from '../../api/models/project-package';
import {ProjectPackageDetailResponse} from '../../api/models/project-package-detail-response';
import {ProjectPackageFilter} from '../../api/models/project-package-filter';
import {ProjectPackageSortField} from '../../api/models/project-package-sort-field';
import {PackageStatus} from '../../api/models/package-status';
import {BranchOption} from '../../shared/components/branch-filter/branch-filter.component';
import {PackageFilter} from '../../api/models/package-filter';

@Injectable({
  providedIn: 'root'
})
export class DependencyStore {
  loading = signal(false);
  dependencies = signal<ProjectPackage[]>([]);
  // dependency detail
  showDependency = signal(false);
  loadingDependency = signal(false);
  packageDetail = signal<ProjectPackageDetailResponse | null>(null);
  filter: PackageFilter = {
    sortBy: ProjectPackageSortField.RiskLevel,
    size: 20,
    desc: true,
    name: undefined,
    page: 1,
    severity: undefined,
    status: PackageStatus.Open
  }
  // paginator
  currentPage = signal(1);
  pageSize = signal(20);
  totalRecords = signal(0);
  firstRecord = computed(() => {
    return (this.currentPage() - 1) * this.pageSize();
  });
  // filter options
}
