import {Component, OnDestroy} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {Router, RouterLink, RouterOutlet} from "@angular/router";
import {Subject} from 'rxjs';
import {MenuItem} from 'primeng/api';
import {Panel} from 'primeng/panel';
import {Tab, TabList, Tabs} from 'primeng/tabs';

@Component({
  selector: 'app-setting',
  standalone: true,
  imports: [
    NgIcon,
    RouterOutlet,
    RouterLink,
    Panel,
    Tab,
    TabList,
    Tabs
  ],
  templateUrl: './setting.component.html',
})
export class SettingComponent implements OnDestroy {
  constructor(
    private router: Router,
  ) {
    console.log(router.url);
    if (!this.regexBaseUrl.test(router.url)) {
      console.log(this.navItems[0].routerLink);
      this.router.navigateByUrl(this.navItems[0].routerLink).then();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  navItems: MenuItem[] = [
    {
      label: 'General',
      icon: 'heroAdjustmentsHorizontal',
      routerLink: '/setting/general',
    },
    {
      label: 'Access Token',
      routerLink: '/setting/ci-token',
      icon: 'token',
    },
    {
      label: 'Integration',
      routerLink: '/setting/integration',
      icon: 'plugin',
    }
  ]
  private destroy$ = new Subject();
  private regexBaseUrl = new RegExp('^\\/setting\\/[^\\/]+$');
  activeTab = 0;
  tabs: MenuItem[] = [
    {
      label: 'General',
      icon: 'heroAdjustmentsHorizontal',
      routerLink: '/setting/general',
    },
    {
      label: 'Access Token',
      routerLink: '/setting/ci-token',
      icon: 'token',
    },
    {
      label: 'Integration',
      routerLink: '/setting/integration',
      icon: 'plugin',
    }
  ];
}
