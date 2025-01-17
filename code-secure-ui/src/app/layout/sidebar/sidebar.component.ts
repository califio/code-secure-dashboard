import {Component, OnInit} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {NgClass} from "@angular/common";
import {SidebarMenuComponent} from "./sidebar-menu/sidebar-menu.component";
import {MenuService} from "../../core/menu";

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    NgIcon,
    NgClass,
    SidebarMenuComponent
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent implements OnInit{
  constructor(
    public menuService: MenuService
  ) {
  }
  ngOnInit(): void {
  }
  toggleSidebar() {
    this.menuService.toggleSidebar();
  }
}
