import {Routes} from '@angular/router';
import {ListProjectComponent} from './list-project/list-project.component';
import {ProjectComponent} from './project/project.component';

export const routes: Routes = [
  {
    path: '',
    component: ListProjectComponent,
  },
  {
    path: ':projectId',
    component: ProjectComponent,
    children: [
      {
        path: 'scan',
        loadComponent: () => import('./project/scan/scan.component').then(x => x.ScanComponent)
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
