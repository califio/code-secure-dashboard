import { Component } from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {MenuService} from "../../core/menu";
import {NavbarMobileComponent} from "./navbar-mobile/navbar-mobile.component";
import {ProfileMenuComponent} from "./profile-menu/profile-menu.component";

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    NgIcon,
    NavbarMobileComponent,
    ProfileMenuComponent
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  constructor(
    private menuService: MenuService
  ) {
  }

  toggleMobileMenu() {
    this.menuService.showMobileMenu = !this.menuService.showMobileMenu;
  }
}
