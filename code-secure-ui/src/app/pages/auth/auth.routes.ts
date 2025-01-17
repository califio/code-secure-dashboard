import {Routes} from '@angular/router';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./login/login.component').then(x => x.LoginComponent),
  },
  {
    path: 'confirm-email',
    loadComponent: () => import('./confirm-email/confirm-email.component').then(x => x.ConfirmEmailComponent),
  },
  {
    path: 'forgot-password',
    loadComponent: () => import('./forgot-password/forgot-password.component').then(x => x.ForgotPasswordComponent),
  },
  {
    path: 'reset-password',
    loadComponent: () => import('./reset-password/reset-password.component').then(x => x.ResetPasswordComponent),
  },
  {
    path: 'two-step',
    loadComponent: () => import('./two-step-verification/two-step-verification.component').then(x => x.TwoStepVerificationComponent),
  }
]
