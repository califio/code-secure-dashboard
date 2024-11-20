import {Injectable, signal} from '@angular/core';
import {ProjectPackage} from '../../../../api/models/project-package';
import {ProjectPackageFilter} from '../../../../api/models/project-package-filter';

@Injectable({
  providedIn: 'root'
})
export class DependencyStore {
  loading = signal(false);
  currentPage = signal(0);
  totalPage = signal(0);
  dependencies = signal<ProjectPackage[]>([]);
  filter: ProjectPackageFilter = {
    desc: true,
    name: '',
    page: 1,
    size: 20
  }
}
