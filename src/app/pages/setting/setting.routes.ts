import {Routes} from '@angular/router';
import {SettingComponent} from './setting.component';

export const routes: Routes = [
  {
    path: '',
    component: SettingComponent,
    children: [
      {
        path: 'ci-token',
        loadComponent: () => import('./ci-token/ci-token.component').then(x => x.CiTokenComponent)
      }
    ],
  }
]
