import { Component } from '@angular/core';
import {RouterLink, RouterLinkActive, RouterOutlet} from '@angular/router';
import {NgIcon} from '@ng-icons/core';

@Component({
  selector: 'app-setting',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive,
    NgIcon,
    RouterOutlet
  ],
  templateUrl: './setting.component.html',
  styleUrl: './setting.component.scss'
})
export class SettingComponent {
  menu = false;

}
