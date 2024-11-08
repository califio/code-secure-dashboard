import {Routes} from '@angular/router';
import {NotFoundComponent} from './not-found/not-found.component';
import {AccessDeniedComponent} from './access-denied/access-denied.component';
import {InternalServerErrorComponent} from './internal-server-error/internal-server-error.component';

export const routes: Routes = [
  {
    path: '404',
    component: NotFoundComponent,
  },
  {
    path: '403',
    component: AccessDeniedComponent,
  },
  {
    path: '500',
    component: InternalServerErrorComponent,
  },
]
