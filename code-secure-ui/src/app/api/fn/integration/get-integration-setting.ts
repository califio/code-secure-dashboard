/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { IntegrationSetting } from '../../models/integration-setting';

export interface GetIntegrationSetting$Params {
}

export function getIntegrationSetting(http: HttpClient, rootUrl: string, params?: GetIntegrationSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<IntegrationSetting>> {
  const rb = new RequestBuilder(rootUrl, getIntegrationSetting.PATH, 'get');
  if (params) {
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<IntegrationSetting>;
    })
  );
}

getIntegrationSetting.PATH = '/api/integration';
