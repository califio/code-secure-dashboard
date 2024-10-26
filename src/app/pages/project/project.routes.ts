import {Routes} from '@angular/router';
import {ListComponent} from './list/list.component';
import {ProjectComponent} from './project/project.component';

export const routes: Routes = [
  {
    path: '',
    component: ListComponent,
  },
  {
    path: ':slug',
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
        loadComponent: () => import('./project/setting/setting.component').then(x => x.SettingComponent)
      }
    ]
  }
]
