import {Routes} from '@angular/router';
import {LoginComponent} from './login/login.component';
import {ForgotPasswordComponent} from './forgot-password/forgot-password.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'confirm-email',
    loadComponent: () => import('./confirm-email/confirm-email.component').then(x => x.ConfirmEmailComponent),
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent,
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
