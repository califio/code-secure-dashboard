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
      },
      {
        path: 'authentication',
        loadComponent: () => import('./authentication/authentication.component').then(x => x.AuthenticationComponent)
      },
      {
        path: 'mail',
        loadComponent: () => import('./mail/mail.component').then(x => x.MailComponent)
      },
      {
        path: 'sla',
        loadComponent: () => import('./sla/sla.component').then(x => x.SlaComponent)
      }
    ],
  }
]
