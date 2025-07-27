import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import {MenuItem, PrimeIcons} from 'primeng/api';
import { AppMenuItem } from './app.menu-item';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [CommonModule, AppMenuItem, RouterModule],
  template: `<ul class="layout-menu">
        <ng-container *ngFor="let item of model; let i = index">
            <li app-menu-item *ngIf="!item.separator" [item]="item" [index]="i" [root]="true"></li>
            <li *ngIf="item.separator" class="menu-separator"></li>
        </ng-container>
    </ul> `
})
export class AppMenu implements  OnInit {
  model: MenuItem[] = [];

  ngOnInit() {
    this.model = [
      {
        label: 'Application',
        items: [
          { label: 'Dashboard', icon: PrimeIcons.OBJECTS_COLUMN, routerLink: ['/dashboard'] },
          { label: 'Project', icon: PrimeIcons.FOLDER, routerLink: ['/project'] },
          { label: 'Finding', icon: PrimeIcons.SEARCH, routerLink: ['/finding'] },
          { label: 'Dependency', icon: PrimeIcons.SITEMAP, routerLink: ['/dependency'] },
        ]
      },
      {
        label: 'Admin',
        items: [
          { label: 'Rule', icon: PrimeIcons.SHIELD, routerLink: ['/rule'] },
          { label: 'User Manager', icon: PrimeIcons.USERS, routerLink: ['/user'] },
          { label: 'Setting', icon: PrimeIcons.COG, routerLink: ['/setting'] },
        ]
      }
    ];
  }
}
