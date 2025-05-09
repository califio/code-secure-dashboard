/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { TeamsAlertSetting } from '../../models/teams-alert-setting';

export interface GetTeamsIntegrationSetting$Params {
}

export function getTeamsIntegrationSetting(http: HttpClient, rootUrl: string, params?: GetTeamsIntegrationSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<TeamsAlertSetting>> {
  const rb = new RequestBuilder(rootUrl, getTeamsIntegrationSetting.PATH, 'get');
  if (params) {
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<TeamsAlertSetting>;
    })
  );
}

getTeamsIntegrationSetting.PATH = '/api/teams-integration';
