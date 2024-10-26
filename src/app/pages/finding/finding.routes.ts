import {Routes} from '@angular/router';

export const routes: Routes = [
  {
    path: ':id',
    loadComponent: () => import('./finding/finding.component').then(x => x.FindingComponent),
  }
]
