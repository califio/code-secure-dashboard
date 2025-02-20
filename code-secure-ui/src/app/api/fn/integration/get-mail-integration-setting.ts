/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { MailAlertSetting } from '../../models/mail-alert-setting';

export interface GetMailIntegrationSetting$Params {
}

export function getMailIntegrationSetting(http: HttpClient, rootUrl: string, params?: GetMailIntegrationSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<MailAlertSetting>> {
  const rb = new RequestBuilder(rootUrl, getMailIntegrationSetting.PATH, 'get');
  if (params) {
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<MailAlertSetting>;
    })
  );
}

getMailIntegrationSetting.PATH = '/api/integration/mail';
