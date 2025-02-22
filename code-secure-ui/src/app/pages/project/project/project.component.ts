import {Component, computed, OnDestroy, OnInit} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {NavigationEnd, Router, RouterLink, RouterOutlet} from "@angular/router";
import {getPathParam} from '../../../core/router';
import {Subject, switchMap, takeUntil} from 'rxjs';
import {ProjectStore} from './project.store';
import {ProjectService} from '../../../api/services';
import {MenuItem} from 'primeng/api';
import {Tab, TabList, Tabs} from 'primeng/tabs';
import {Panel} from 'primeng/panel';

@Component({
  selector: 'app-project',
  standalone: true,
  imports: [
    NgIcon,
    RouterOutlet,
    RouterLink,
    Tabs,
    TabList,
    Tab,
    Panel,
  ],
  templateUrl: './project.component.html',
  styleUrl: './project.component.scss'
})
export class ProjectComponent implements OnInit, OnDestroy {
  activeTab = 0;
  tabs = computed<MenuItem[]>(() => {
    const projectId = this.store.projectId();
    return [
      {
        label: 'Overview',
        routerLink: `/project/${projectId}/scan`,
        icon: 'scan',
        routerLinkActiveOptions: {exact: false}
      },
      {
        label: 'Finding',
        routerLink: `/project/${projectId}/finding`,
        icon: 'bug',
        routerLinkActiveOptions: {exact: false}
      },
      {
        label: 'Dependency',
        routerLink: `/project/${projectId}/dependency`,
        icon: 'inventory',
        routerLinkActiveOptions: {exact: false}
      },
      {
        label: 'Setting',
        routerLink: `/project/${projectId}/setting`,
        icon: 'setting',
        routerLinkActiveOptions: {exact: false}
      }
    ]
  });

  constructor(
    private router: Router,
    private store: ProjectStore,
    private projectService: ProjectService,
  ) {

    getPathParam('projectId').pipe(
      switchMap(projectId => {
        this.store.projectId.set(projectId);
        return this.projectService.getProjectInfo({
          projectId: projectId
        })
      }),
      takeUntil(this.destroy$)
    ).subscribe(project => {
      this.store.project.set(project);
    })
    if (this.regexBaseUrl.test(this.router.url)) {
      this.router.navigate(['/project', this.store.projectId(), 'scan']).then();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  ngOnInit(): void {
    this.activeTab = this.tabs().findIndex(tab => this.router.url.includes(tab.routerLink));
    this.router.events.pipe(
      takeUntil(this.destroy$)
    ).subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.activeTab = this.tabs().findIndex(tab => event.url.includes(tab.routerLink));
      }
    });
  }

  private destroy$ = new Subject();
  private regexBaseUrl = new RegExp('^\\/project\\/[^\\/]+$');
}
