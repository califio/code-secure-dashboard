/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { TeamsSetting } from '../../models/teams-setting';

export interface UpdateTeamsIntegrationProject$Params {
  projectId: string;
      body?: TeamsSetting
}

export function updateTeamsIntegrationProject(http: HttpClient, rootUrl: string, params: UpdateTeamsIntegrationProject$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
  const rb = new RequestBuilder(rootUrl, updateTeamsIntegrationProject.PATH, 'post');
  if (params) {
    rb.path('projectId', params.projectId, {"style":"simple"});
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'text', accept: '*/*', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
    })
  );
}

updateTeamsIntegrationProject.PATH = '/api/project/{projectId}/integration/teams';
