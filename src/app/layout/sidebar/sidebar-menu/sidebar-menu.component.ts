import {Component, OnInit} from '@angular/core';
import {NgClass, NgTemplateOutlet} from "@angular/common";
import {MenuService, SubMenuItem} from "../../../core/menu";
import {NgIcon} from "@ng-icons/core";
import {RouterLink, RouterLinkActive} from "@angular/router";
import {SidebarSubmenuComponent} from "../sidebar-submenu/sidebar-submenu.component";

@Component({
  selector: 'app-sidebar-menu',
  standalone: true,
  imports: [
    NgClass,
    NgIcon,
    NgTemplateOutlet,
    RouterLink,
    RouterLinkActive,
    SidebarSubmenuComponent
  ],
  templateUrl: './sidebar-menu.component.html',
  styleUrl: './sidebar-menu.component.scss'
})
export class SidebarMenuComponent implements OnInit{
  constructor(
    public menuService: MenuService
  ) {
  }

  ngOnInit(): void {
  }

  toggleMenu(subMenu: SubMenuItem) {
    this.menuService.toggleMenu(subMenu);
  }
}
