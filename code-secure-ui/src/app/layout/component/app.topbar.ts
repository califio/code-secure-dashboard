import {Component} from '@angular/core';
import {MenuItem} from 'primeng/api';
import {RouterModule} from '@angular/router';
import {CommonModule} from '@angular/common';
import {StyleClassModule} from 'primeng/styleclass';
import {AppConfigurator} from './app.configurator';
import {LayoutService} from '../layout.service';
import {AppProfileMenu} from './app.profile-menu';
import {NgIcon} from '@ng-icons/core';

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [RouterModule, CommonModule, StyleClassModule, AppConfigurator, AppProfileMenu, NgIcon],
  template: `
    <div class="layout-topbar">
      <div class="layout-topbar-logo-container">
        <button class="layout-menu-button layout-topbar-action" (click)="layoutService.onMenuToggle()">
          <i class="pi pi-bars"></i>
        </button>
        <a class="layout-topbar-logo" routerLink="/">
          <ng-icon name="logo" size="28"></ng-icon>
          <span>CODE SECURE</span>
        </a>
      </div>

      <div class="layout-topbar-actions">
        <div class="layout-config-menu">
          <button type="button" class="layout-topbar-action" (click)="toggleDarkMode()">
            <i
              [ngClass]="{ 'pi ': true, 'pi-moon': layoutService.isDarkTheme(), 'pi-sun': !layoutService.isDarkTheme() }"></i>
          </button>
          <div class="relative">
            <button
              class="layout-topbar-action layout-topbar-action-highlight"
              pStyleClass="@next"
              enterFromClass="hidden"
              enterActiveClass="animate-scalein"
              leaveToClass="hidden"
              leaveActiveClass="animate-fadeout"
              [hideOnOutsideClick]="true"
            >
              <i class="pi pi-palette"></i>
            </button>
            <app-configurator/>
          </div>
          <div class="relative">
            <button
              class="layout-topbar-action layout-topbar-action-highlight"
              pStyleClass="@next"
              enterFromClass="hidden"
              enterActiveClass="animate-scalein"
              leaveToClass="hidden"
              leaveActiveClass="animate-fadeout"
              [hideOnOutsideClick]="true"
            >
              <i class="pi pi-user"></i>
            </button>
            <app-profile-menu></app-profile-menu>
          </div>
        </div>
      </div>
    </div>`
})
export class AppTopbar {
  items!: MenuItem[];

  constructor(public layoutService: LayoutService) {
  }

  toggleDarkMode() {
    this.layoutService.layoutConfig.update((state) => ({...state, darkTheme: !state.darkTheme}));
  }
}
