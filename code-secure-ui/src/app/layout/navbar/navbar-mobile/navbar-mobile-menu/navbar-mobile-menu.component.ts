import { Component } from '@angular/core';
import {MenuService, SubMenuItem} from "../../../../core/menu";
import {NgClass} from "@angular/common";
import {NgIcon} from "@ng-icons/core";
import {RouterLink, RouterLinkActive} from "@angular/router";
import {NavbarMobileSubmenuComponent} from "../navbar-mobile-submenu/navbar-mobile-submenu.component";

@Component({
  selector: 'app-navbar-mobile-menu',
  standalone: true,
  imports: [
    NgClass,
    NgIcon,
    RouterLinkActive,
    RouterLink,
    NavbarMobileSubmenuComponent
  ],
  templateUrl: './navbar-mobile-menu.component.html',
  styleUrl: './navbar-mobile-menu.component.scss'
})
export class NavbarMobileMenuComponent {
  constructor(
    public menuService: MenuService
  ) {
  }

  public toggleMenu(subMenu: SubMenuItem) {
    this.menuService.toggleMenu(subMenu);
  }

  closeMenu() {
    this.menuService.showMobileMenu = false;
  }
}
