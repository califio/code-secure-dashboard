import { Component } from '@angular/core';
import {MenuService} from "../../../core/menu";
import {NgClass} from "@angular/common";
import {NgIcon} from "@ng-icons/core";
import {NavbarMobileMenuComponent} from "./navbar-mobile-menu/navbar-mobile-menu.component";

@Component({
  selector: 'app-navbar-mobile',
  standalone: true,
  imports: [
    NgClass,
    NgIcon,
    NavbarMobileMenuComponent
  ],
  templateUrl: './navbar-mobile.component.html',
  styleUrl: './navbar-mobile.component.scss'
})
export class NavbarMobileComponent {
  constructor(
    public menuService: MenuService
  ) {
  }

  toggleMobileMenu() {
    this.menuService.toggleMobileMenu();
  }
}
