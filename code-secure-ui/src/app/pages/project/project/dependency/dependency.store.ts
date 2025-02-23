import {computed, Injectable, signal} from '@angular/core';
import {ProjectPackage} from '../../../../api/models/project-package';
import {ProjectPackageFilter} from '../../../../api/models/project-package-filter';
import {ProjectPackageSortField} from '../../../../api/models/project-package-sort-field';

@Injectable({
  providedIn: 'root'
})
export class DependencyStore {
  loading = signal(false);
  dependencies = signal<ProjectPackage[]>([]);
  // dependency detail
  showDependency = signal(false);
  loadingDependency = signal(false);
  dependency = signal<null | ProjectPackage>(null);
  filter: ProjectPackageFilter = {}
  // paginator
  currentPage = signal(1);
  pageSize = signal(20);
  totalRecords = signal(0);
  firstRecord = computed(() => {
    return (this.currentPage() - 1) * this.pageSize();
  })
}
