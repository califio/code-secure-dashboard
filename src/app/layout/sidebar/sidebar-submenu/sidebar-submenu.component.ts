import {Component, input} from '@angular/core';
import {NgClass, NgTemplateOutlet} from "@angular/common";
import {MenuService, SubMenuItem} from "../../../core/menu";
import {NgIcon} from "@ng-icons/core";
import {RouterLink, RouterLinkActive} from "@angular/router";

@Component({
  selector: 'app-sidebar-submenu',
  standalone: true,
  imports: [
    NgClass,
    NgTemplateOutlet,
    NgIcon,
    RouterLinkActive,
    RouterLink
  ],
  templateUrl: './sidebar-submenu.component.html',
  styleUrl: './sidebar-submenu.component.scss'
})
export class SidebarSubmenuComponent {
  item = input<SubMenuItem>({})
  constructor(
    public menuService: MenuService
  ) {
  }

  toggleMenu(sub: SubMenuItem) {

  }
}
