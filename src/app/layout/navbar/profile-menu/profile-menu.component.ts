import {Component, OnInit} from '@angular/core';
import {ThemeService} from "../../../core/theme";
import {Router, RouterLink} from "@angular/router";
import {NgIcon} from "@ng-icons/core";
import {NgClass} from "@angular/common";
import {ClickOutsideDirective} from "../../../shared/directives/click-outside.directive";
import {AuthService} from '../../../api/services/auth.service';
import {AuthStore} from '../../../core/auth/auth.store';
import {AvatarComponent} from '../../../shared/components/ui/avatar/avatar.component';

@Component({
  selector: 'app-profile-menu',
  standalone: true,
  imports: [
    RouterLink,
    NgIcon,
    NgClass,
    ClickOutsideDirective,
    AvatarComponent,
  ],
  templateUrl: './profile-menu.component.html',
  styleUrl: './profile-menu.component.scss',
})
export class ProfileMenuComponent implements OnInit {
  isOpen = false;
  themeColors = [
    {
      name: 'base',
      code: '#e11d48',
    },
    {
      name: 'yellow',
      code: '#f59e0b',
    },
    {
      name: 'green',
      code: '#22c55e',
    },
    {
      name: 'blue',
      code: '#3b82f6',
    },
    {
      name: 'orange',
      code: '#ea580c',
    },
    {
      name: 'red',
      code: '#cc0022',
    },
    {
      name: 'violet',
      code: '#6d28d9',
    },
  ];

  themeMode = ['light', 'dark'];

  constructor(
    public themeService: ThemeService,
    private authService: AuthService,
    public authStore: AuthStore,
    private router: Router
  ) {
  }

  ngOnInit(): void {}

  public toggleMenu(): void {
    this.isOpen = !this.isOpen;
  }

  toggleThemeMode() {
    this.themeService.theme.update((theme) => {
      const mode = !this.themeService.isDark ? 'dark' : 'light';
      return { ...theme, mode: mode };
    });
  }

  toggleThemeColor(color: string) {
    this.themeService.theme.update((theme) => {
      return { ...theme, color: color };
    });
  }

  logout() {
    this.authService.logout({
      body: {
        token: this.authStore.refreshToken
      }
    }).subscribe(success => {
      if (success) {
        this.authStore.clearSession();
        this.router.navigate(["/auth", "login"]).then();
      }
    })
  }
}
