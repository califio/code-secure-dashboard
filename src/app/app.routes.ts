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
        path: 'dashboard',
        loadComponent: () => import('./pages/dashboard/dashboard.component').then((x) => x.DashboardComponent)
      },
      {
        path: 'assets',
        loadChildren: () => import('./pages/assets/assets.routes').then((x) => x.routes)
      },
      {
        path: 'project',
        loadChildren: () => import('./pages/project/project.routes').then((x) => x.routes)
      },
      {
        path: 'finding',
        loadChildren: () => import('./pages/finding/finding.routes').then((x) => x.routes)
      },
    ]
  },
  {
    path: 'auth',
    component: AuthComponent,
    loadChildren: () => import('./pages/auth/auth.routes').then((x) => x.routes)
  },
];
