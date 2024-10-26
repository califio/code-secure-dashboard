import {Injectable, signal} from '@angular/core';
import {ProjectSummary} from './project.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectStoreService {
  slug = signal('')
  currentProject = signal<ProjectSummary>({finding: 0, id: undefined, last_scan: undefined, name: '', slug: ''})
  constructor() { }
}
