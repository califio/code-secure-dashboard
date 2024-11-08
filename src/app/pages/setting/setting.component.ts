import {Component, OnDestroy} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {ActivatedRoute, NavigationStart, Router, RouterLink, RouterLinkActive, RouterOutlet} from "@angular/router";
import {NavItem} from '../../core/menu';
import {filter, Subject, takeUntil} from 'rxjs';

@Component({
  selector: 'app-setting',
  standalone: true,
  imports: [
    NgIcon,
    RouterLinkActive,
    RouterOutlet,
    RouterLink
  ],
  templateUrl: './setting.component.html',
  styleUrl: './setting.component.scss'
})
export class SettingComponent implements OnDestroy {

  constructor(
    private router: Router,
  ) {
    if (!this.regexBaseUrl.test(router.url)) {
      this.router.navigateByUrl(this.navItems[0].route).then();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }
  navItems: NavItem[] = [
    {
      label: 'CI Token',
      route: '/setting/ci-token',
      icon: 'rocket',
      count: 1
    },
    {
      label: 'Vulnerabilities',
      route: '/setting/acd',
      icon: 'bug',
      count: 123
    }
  ]
  private destroy$ = new Subject();
  private regexBaseUrl = new RegExp('^\\/setting\\/[^\\/]+$');
}
