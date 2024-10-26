import {computed, Injectable, signal} from '@angular/core';
import {MenuItem, SubMenuItem} from "./menu.model";
import {items} from "./menu";

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  private _menuItems = signal<MenuItem[]>([]);
  public menuItems = computed(() => this._menuItems());
  showSidebar = true;
  showMobileMenu = false;
  constructor() {
    this._menuItems.set(items)
  }

  public toggleSidebar() {
    this.showSidebar = !this.showSidebar;
  }

  public toggleMobileMenu() {
    this.showMobileMenu = !this.showMobileMenu;
  }

  toggleMenu(subMenu: SubMenuItem) {
    subMenu.expanded = !subMenu.expanded;
  }
}
