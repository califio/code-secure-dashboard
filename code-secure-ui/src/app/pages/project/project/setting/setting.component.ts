import { Component } from '@angular/core';
import {Router, RouterLink, RouterLinkActive, RouterOutlet} from '@angular/router';
import {NgIcon} from '@ng-icons/core';
import {ProjectStore} from '../project.store';
import {ProjectService} from '../../../../api/services/project.service';
import {ProjectSetting} from '../../../../api/models/project-setting';

@Component({
  selector: 'app-setting',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive,
    NgIcon,
    RouterOutlet
  ],
  templateUrl: './setting.component.html',
  styleUrl: './setting.component.scss'
})
export class SettingComponent {
  projectId = '';
  constructor(
    private projectStore: ProjectStore,
    private projectService: ProjectService,
    private router: Router,
  ) {
    this.projectId = this.projectStore.projectId();
    this.projectService.getProjectSetting({
      projectId: this.projectId
    }).subscribe(setting => {
      this.projectStore.projectSetting.set(setting);
    })
    if (this.router.url.endsWith("/setting")) {
      this.router.navigate(['/project', this.projectId, 'setting', 'member']).then();
    }
  }
  routerLink(route: string): string {
    return `/project/${this.projectId}/setting/${route}`;
  }
}
