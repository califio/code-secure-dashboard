import {Component, OnDestroy, OnInit} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {Router, RouterLink, RouterLinkActive, RouterOutlet} from "@angular/router";
import {NavItem} from '../../../core/menu';
import {getPathParam} from '../../../core/router';
import {Subject, takeUntil} from 'rxjs';
import {ProjectStore} from './project-store';

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
      label: 'Configurations',
      route: 'setting',
      icon: 'setting',
    }
  ]
  slug = '';
  constructor(
    private router: Router,
    private projectStore: ProjectStore
  ) {
    getPathParam('slug').pipe(
      takeUntil(this.destroy$)
    ).subscribe(slug => {
      this.slug = slug;
      this.projectStore.slug.set(slug);
      if (this.regexBaseUrl.test(this.router.url)) {
        this.router.navigate(['/project', slug, 'scan']).then();
      }
    })
  }
  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }
  ngOnInit(): void {

  }
  routerLink(route: string): string {
    return `/project/${this.slug}/${route}`;
  }
  private destroy$ = new Subject();
  private regexBaseUrl = new RegExp('^\\/project\\/[^\\/]+$');
}
