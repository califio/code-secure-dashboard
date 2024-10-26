import {Component, OnInit} from '@angular/core';
import {ThemeService} from "../../../core/theme";
import {RouterLink} from "@angular/router";
import {NgIcon} from "@ng-icons/core";
import {NgClass} from "@angular/common";
import {ClickOutsideDirective} from "../../../shared/directives/click-outside.directive";

@Component({
  selector: 'app-profile-menu',
  standalone: true,
  imports: [
    RouterLink,
    NgIcon,
    NgClass,
    ClickOutsideDirective,
  ],
  templateUrl: './profile-menu.component.html',
  styleUrl: './profile-menu.component.scss',
})
export class ProfileMenuComponent implements OnInit {
  isOpen = false;
  profileMenu = [
    {
      title: 'Your Profile',
      icon: 'user-circle',
      link: '/profile',
    },
    {
      title: 'Log out',
      icon: 'logout',
      link: '/auth',
    },
  ];

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

  constructor(public themeService: ThemeService) {}

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
}
