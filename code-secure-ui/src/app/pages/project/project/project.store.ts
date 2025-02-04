import {Injectable, signal} from '@angular/core';
import {ProjectInfo} from '../../../api/models/project-info';
import {ProjectSetting} from '../../../api/models/project-setting';

@Injectable({
  providedIn: 'root'
})
export class ProjectStore {
  project = signal<ProjectInfo>({});
  projectSetting = signal<ProjectSetting>({});
  projectId = signal('')
  constructor() { }
}
