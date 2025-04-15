import {Routes} from '@angular/router';
import {SettingComponent} from './setting.component';

export const routes: Routes = [
  {
    path: '',
    component: SettingComponent,
    children: [
      {
        path: 'general',
        loadComponent: () => import('./general/general.component').then(x => x.GeneralComponent)
      },
      {
        path: 'member',
        loadComponent: () => import('./member/member.component').then(x => x.MemberComponent)
      },
      {
        path: 'integration',
        loadComponent: () => import('./integration/integration.component').then(x => x.IntegrationComponent)
      }
    ]
  }
]
