import {Routes} from '@angular/router';
import {SettingComponent} from './setting.component';

export const routes: Routes = [
  {
    path: '',
    component: SettingComponent,
    children: [
      {
        path: 'member',
        loadComponent: () => import('./member/member.component').then(x => x.MemberComponent)
      },
      {
        path: 'threshold',
        loadComponent: () => import('./security-threshold/security-threshold.component').then(x => x.SecurityThresholdComponent)
      }
    ]
  }
]
