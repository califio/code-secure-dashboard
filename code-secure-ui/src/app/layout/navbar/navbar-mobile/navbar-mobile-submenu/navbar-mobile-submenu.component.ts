import {Component, input} from '@angular/core';
import {NgClass} from "@angular/common";
import {NgIcon} from "@ng-icons/core";
import {RouterLink, RouterLinkActive} from "@angular/router";
import {MenuService, SubMenuItem} from "../../../../core/menu";

@Component({
  selector: 'app-navbar-mobile-submenu',
  standalone: true,
  imports: [
    NgClass,
    NgIcon,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './navbar-mobile-submenu.component.html',
  styleUrl: './navbar-mobile-submenu.component.scss'
})
export class NavbarMobileSubmenuComponent {
  item = input<SubMenuItem>({})

  constructor(
    private menuService: MenuService
  ) {
  }

  toggleMenu(sub: SubMenuItem) {
    this.menuService.toggleMenu(sub);
  }

  closeMobileMenu() {
    this.menuService.showMobileMenu = false;
  }
}
