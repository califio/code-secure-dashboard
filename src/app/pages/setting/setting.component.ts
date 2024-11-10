import {Component, OnDestroy} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {Router, RouterLink, RouterLinkActive, RouterOutlet} from "@angular/router";
import {NavItem} from '../../core/menu';
import {Subject} from 'rxjs';

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
    },
    {
      label: 'Configuration',
      route: '/setting/configuration',
      icon: 'setting',
    }
  ]
  private destroy$ = new Subject();
  private regexBaseUrl = new RegExp('^\\/setting\\/[^\\/]+$');
}
