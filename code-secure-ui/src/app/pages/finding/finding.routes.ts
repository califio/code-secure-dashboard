import {Routes} from '@angular/router';
import {ListFindingComponent} from './list-finding/list-finding.component';
export const routes: Routes = [
  {
    path: '',
    component: ListFindingComponent,
  },
  {
    path: ':id',
    loadComponent: () => import('./finding/finding.component').then(x => x.FindingComponent),
  }
]
