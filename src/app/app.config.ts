import {ApplicationConfig, importProvidersFrom, inject, provideZoneChangeDetection} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {HTTP_INTERCEPTORS, HttpClient, provideHttpClient, withInterceptorsFromDi} from "@angular/common/http";
import {provideNgIconLoader, provideNgIconsConfig, withContentSecurityPolicy} from "@ng-icons/core";
import {TimeagoModule} from 'ngx-timeago';
import {AuthInterceptor} from './core/auth/auth.interceptor';
import {ApiModule} from './api/api.module';
import {environment} from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideNgIconsConfig({}, withContentSecurityPolicy()),
    provideNgIconLoader(name => {
      const http = inject(HttpClient);
      return http.get(`/icons/${name}.svg`, { responseType: 'text' });
    }),
    importProvidersFrom(
      TimeagoModule.forRoot(),
      ApiModule.forRoot({rootUrl: environment.baseUrl})
    ),
    provideHttpClient(
      withInterceptorsFromDi()
    ),
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
  ]
};
