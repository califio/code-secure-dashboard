import {Injectable, signal} from '@angular/core';
import {ProjectInfo} from '../../../api/models/project-info';

@Injectable({
  providedIn: 'root'
})
export class ProjectStore {
  project = signal<ProjectInfo>({});
  slug = signal('')
  constructor() { }
}
