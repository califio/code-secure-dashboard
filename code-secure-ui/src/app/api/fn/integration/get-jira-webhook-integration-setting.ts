/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { JiraWebhookSetting } from '../../models/jira-webhook-setting';

export interface GetJiraWebhookIntegrationSetting$Params {
}

export function getJiraWebhookIntegrationSetting(http: HttpClient, rootUrl: string, params?: GetJiraWebhookIntegrationSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<JiraWebhookSetting>> {
  const rb = new RequestBuilder(rootUrl, getJiraWebhookIntegrationSetting.PATH, 'get');
  if (params) {
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<JiraWebhookSetting>;
    })
  );
}

getJiraWebhookIntegrationSetting.PATH = '/api/integration/jira-webhook';
