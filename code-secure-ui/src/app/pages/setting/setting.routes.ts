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
        path: 'ci-token',
        loadComponent: () => import('./ci-token/ci-token.component').then(x => x.CiTokenComponent)
      },
      {
        path: 'integration',
        loadComponent: () => import('./integration/integration.component').then((x) => x.IntegrationComponent)
      },
    ],
  }
]
