import {Component, computed, OnInit} from '@angular/core';
import {Router, RouterLink, RouterOutlet} from '@angular/router';
import {NgIcon} from '@ng-icons/core';
import {ProjectStore} from '../project.store';
import {ProjectService} from '../../../../api/services/project.service';
import {Tab, TabList, Tabs} from "primeng/tabs";
import {MenuItem} from 'primeng/api';

@Component({
  selector: 'app-setting',
  standalone: true,
  imports: [
    RouterLink,
    NgIcon,
    RouterOutlet,
    Tab,
    TabList,
    Tabs
  ],
  templateUrl: './setting.component.html',
  styleUrl: './setting.component.scss'
})
export class SettingComponent implements OnInit {
  activeTab = 0;
  tabs = computed<MenuItem[]>(() => {
    const projectId = this.projectStore.projectId();
    return [
      {
        label: 'General',
        routerLink: `/project/${projectId}/setting/general`,
        icon: 'heroAdjustmentsHorizontal',
      },
      {
        label: 'Member',
        routerLink: `/project/${projectId}/setting/member`,
        icon: 'users',
      },
      {
        label: 'Integration',
        routerLink: `/project/${projectId}/setting/integration`,
        icon: 'plugin',
      },
    ]
  });

  constructor(
    private projectStore: ProjectStore,
    private projectService: ProjectService,
    private router: Router,
  ) {
    if (this.router.url.endsWith("/setting")) {
      this.router.navigate(['/project', this.projectStore.projectId(), 'setting', 'general']).then();
    }
  }

  ngOnInit(): void {
    this.activeTab = this.tabs().findIndex(tab => this.router.url.includes(tab.routerLink));
  }
}
