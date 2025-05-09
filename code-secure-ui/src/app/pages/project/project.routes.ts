import {Routes} from '@angular/router';
import {ListComponent} from './list/list.component';
import {ProjectComponent} from './project/project.component';

export const routes: Routes = [
  {
    path: '',
    component: ListComponent,
  },
  {
    path: ':projectId',
    component: ProjectComponent,
    children: [
      {
        path: 'overview',
        loadComponent: () => import('./project/overview/overview.component').then(x => x.OverviewComponent)
      },
      {
        path: 'finding',
        loadComponent: () => import('./project/finding/finding.component').then(x => x.FindingComponent)
      },
      {
        path: 'dependency',
        loadComponent: () => import('./project/dependency/dependency.component').then(x => x.DependencyComponent)
      },
      {
        path: 'setting',
        loadChildren: () => import('./project/setting/setting.routes').then((x) => x.routes),
      }
    ]
  }
]
