import {Routes} from '@angular/router';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./login/login.component').then(x => x.LoginComponent),
  },
  {
    path: 'oidc/callback',
    loadComponent: () => import('./oidc-callback/oidc-callback.component').then(x => x.OidcCallbackComponent),
  },
  {
    path: 'two-step',
    loadComponent: () => import('./two-step-verification/two-step-verification.component').then(x => x.TwoStepVerificationComponent),
  }
]
