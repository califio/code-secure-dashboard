import {Component, computed, OnInit} from '@angular/core';
import {Router, RouterLink} from "@angular/router";
import {AuthService} from '../../api/services/auth.service';
import {AuthStore} from '../../core/auth/auth.store';
import {finalize} from 'rxjs';
import {Avatar} from 'primeng/avatar';
import {Divider} from 'primeng/divider';

@Component({
  selector: 'app-profile-menu',
  standalone: true,
  imports: [
    RouterLink,
    Avatar,
    Divider,
  ],
  template: `
    <div class="flex flex-col gap-1">
      <div class="flex-row flex items-center gap-2">
        <p-avatar
          size="large"
          [image]="authStore.currentUser().avatar ?? undefined"
          [label]="label()"></p-avatar>
        <div class="flex flex-col font-semibold gap-1">
          <span>{{ authStore.currentUser().fullName || authStore.currentUser().userName }}</span>
          <span class="truncate">{{ authStore.currentUser().email }}</span>
        </div>
      </div>
      <p-divider></p-divider>
      <div class="flex flex-col gap-1">
        <div routerLink="/profile"
             class="inline-flex cursor-pointer items-center gap-2 rounded-md px-3 py-2 hover:bg-surface-100 dark:hover:hover:bg-surface-800">
          <i class="pi pi-user text-lg"></i>
          <span>User Profile</span>
        </div>
        <div (click)="logout()"
             class="inline-flex cursor-pointer items-center gap-2 rounded-md px-3 py-2 hover:bg-surface-100 dark:hover:hover:bg-surface-800">
          <i class="pi pi-power-off text-lg"></i>
          <span>Log out</span>
        </div>
      </div>
    </div>
  `,
  host: {
    class: 'hidden absolute top-[3.25rem] right-0 w-72 p-4 bg-surface-0 dark:bg-surface-900 border border-surface rounded-border origin-top shadow-[0px_3px_5px_rgba(0,0,0,0.02),0px_0px_2px_rgba(0,0,0,0.05),0px_1px_4px_rgba(0,0,0,0.08)]'
  }
})
export class AppProfileMenu implements OnInit {

  label = computed(() => {
    let username = this.authStore.currentUser().userName;
    if (username && username.length > 0) {
      return username.charAt(0).toUpperCase();
    }
    return 'U';
  })

  constructor(
    private authService: AuthService,
    public authStore: AuthStore,
    private router: Router
  ) {
  }

  ngOnInit(): void {
  }

  logout() {
    this.authService.logout({
      body: {
        token: this.authStore.refreshToken ?? ''
      }
    }).pipe(finalize(() => {
      this.authStore.clearSession();
      this.router.navigate(["/auth", "login"]).then();
    })).subscribe()
  }
}
