import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {Router, RouterLink, RouterLinkActive, RouterOutlet} from "@angular/router";
import {NavItem} from '../../../core/menu';
import {getPathParam} from '../../../core/router';
import {Subject, switchMap, takeUntil} from 'rxjs';
import {ProjectStore} from './project.store';
import {ProjectService} from '../../../api/services';

@Component({
  selector: 'app-project',
  standalone: true,
  imports: [
    NgIcon,
    RouterLinkActive,
    RouterOutlet,
    RouterLink
  ],
  templateUrl: './project.component.html',
  styleUrl: './project.component.scss'
})
export class ProjectComponent implements OnInit, OnDestroy {
  navItems: NavItem[] = [
    {
      label: 'Overview',
      route: 'scan',
      icon: 'scan',
      count: 1
    },
    {
      label: 'Vulnerabilities',
      route: 'finding',
      icon: 'bug',
      count: 123
    },
    {
      label: 'Dependency',
      route: 'dependency',
      icon: 'inventory',
      count: 53
    },
    {
      label: 'Settings',
      route: 'setting',
      icon: 'setting',
    }
  ]

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

  }

  routerLink(route: string): string {
    return `/project/${this.store.projectId()}/${route}`;
  }

  private destroy$ = new Subject();
  private regexBaseUrl = new RegExp('^\\/project\\/[^\\/]+$');
}
