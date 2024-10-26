import {Component, OnInit} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {RouterLink, RouterLinkActive, RouterOutlet} from "@angular/router";
import {NavItem} from "../../core/menu";

@Component({
  selector: 'app-asset',
  standalone: true,
  imports: [
    NgIcon,
    RouterOutlet,
    RouterLinkActive,
    RouterLink
  ],
  templateUrl: './assets.component.html',
  styleUrl: './assets.component.scss'
})
export class AssetsComponent implements OnInit {
  navItems: NavItem[] = [
    {
      label: 'Asset Groups',
      route: '/assets',
      icon: 'asset',
      count: 1
    },
    {
      label: 'Inventory',
      route: '/assets/inventory',
      icon: 'inventory',
      count: 123
    },
    {
      label: 'Technologies',
      route: '/assets/technologies',
      icon: 'engine',
      count: 53
    }
  ]
  loading = false;
  ngOnInit(): void {

  }

}
