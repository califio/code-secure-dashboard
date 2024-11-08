import {Routes} from '@angular/router';
import {LayoutComponent} from "./layout/layout.component";
import {AuthComponent} from './pages/auth/auth.component';
import {AuthGuard} from './core/auth/auth.guard';

export const routes: Routes = [
  {path: '', pathMatch: 'full', redirectTo: '/dashboard'},
  {
    path: '',
    canActivate: [AuthGuard],
    component: LayoutComponent,
    children: [
      {
        path: 'error',
        loadChildren: () => import('./pages/error/error.routes').then((x) => x.routes)
      },
      {
        path: 'profile',
        loadComponent: () => import('./pages/profile/profile.component').then((x) => x.ProfileComponent)
      },
      {
        path: 'dashboard',
        loadComponent: () => import('./pages/dashboard/dashboard.component').then((x) => x.DashboardComponent)
      },
      {
        path: 'project',
        loadChildren: () => import('./pages/project/project.routes').then((x) => x.routes)
      },
      {
        path: 'finding',
        loadChildren: () => import('./pages/finding/finding.routes').then((x) => x.routes)
      },
      {
        path: 'user',
        loadComponent: () => import('./pages/user/user.component').then((x) => x.UserComponent)
      },
      {
        path: 'rule',
        loadComponent: () => import('./pages/rule/rule.component').then((x) => x.RuleComponent)
      },
      {
        path: 'setting',
        loadChildren: () => import('./pages/setting/setting.routes').then((x) => x.routes)
      },
    ]
  },
  {
    path: 'auth',
    component: AuthComponent,
    loadChildren: () => import('./pages/auth/auth.routes').then((x) => x.routes)
  },
];
