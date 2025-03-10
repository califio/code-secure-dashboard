/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { TeamsNotificationSetting } from '../../models/teams-notification-setting';
import { TeamsNotificationSettingRequest } from '../../models/teams-notification-setting-request';

export interface UpdateTeamsSetting$Params {
      body?: TeamsNotificationSettingRequest
}

export function updateTeamsSetting(http: HttpClient, rootUrl: string, params?: UpdateTeamsSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<TeamsNotificationSetting>> {
  const rb = new RequestBuilder(rootUrl, updateTeamsSetting.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<TeamsNotificationSetting>;
    })
  );
}

updateTeamsSetting.PATH = '/api/config/teams';
