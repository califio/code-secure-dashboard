import {Component} from '@angular/core';
import {RouterOutlet} from "@angular/router";
import {SidebarComponent} from "./sidebar/sidebar.component";
import {NavbarComponent} from "./navbar/navbar.component";
import {ProfileService} from '../api/services/profile.service';
import {AuthStore} from '../core/auth/auth.store';
import {provideIcons} from '@ng-icons/core';
import {asset, chartPie, users} from '../icons';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    RouterOutlet,
    SidebarComponent,
    NavbarComponent
  ],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  constructor(
    private profileService: ProfileService,
    private authStore: AuthStore
  ) {
    this.profileService.getProfile().subscribe(user => {
      this.authStore.currentUser.set(user);
    })
  }
}
