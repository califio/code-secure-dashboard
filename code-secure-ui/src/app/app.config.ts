import {ApplicationConfig, importProvidersFrom, inject, provideZoneChangeDetection} from '@angular/core';
import {provideRouter, withHashLocation} from '@angular/router';
import {routes} from './app.routes';
import {HTTP_INTERCEPTORS, HttpClient, provideHttpClient, withInterceptorsFromDi} from "@angular/common/http";
import {provideNgIconLoader, provideNgIconsConfig, withContentSecurityPolicy} from "@ng-icons/core";
import {TimeagoModule} from 'ngx-timeago';
import {AuthInterceptor} from './core/auth/auth.interceptor';
import {ApiModule} from './api/api.module';
import {environment} from '../environments/environment';
import {provideMarkdown} from 'ngx-markdown';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {providePrimeNG} from 'primeng/config';
import {MessageService} from 'primeng/api';
import {ToastrService} from './shared/services/toastr.service';
import 'prismjs';
import 'prismjs/components/prism-java.min.js';
import 'prismjs/components/prism-csharp.min.js';
import 'prismjs/components/prism-javascript.min.js';
import 'prismjs/components/prism-typescript.min.js';
import 'prismjs/components/prism-go.min.js';
import 'prismjs/components/prism-python.min.js';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({eventCoalescing: true}),
    provideRouter(routes, withHashLocation()),
    provideMarkdown(),
    provideNgIconsConfig({}, withContentSecurityPolicy()),
    provideNgIconLoader(name => {
      const http = inject(HttpClient);
      return http.get(`/icons/${name}.svg`, {responseType: 'text'});
    }),
    MessageService,
    ToastrService,
    importProvidersFrom(TimeagoModule.forRoot()),
    importProvidersFrom(ApiModule.forRoot({rootUrl: environment.baseUrl})),
    provideHttpClient(
      withInterceptorsFromDi()
    ),
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        options: {
          darkModeSelector: ".dark",
          cssLayer: {
            name: 'primeng',
            order: 'tailwind-base, primeng, tailwind-utilities'
          }
        }
      }
    })
  ]
};
