/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ProjectUserFilter } from '../../models/project-user-filter';
import { ProjectUserPage } from '../../models/project-user-page';

export interface GetProjectUsers$Params {
  projectId: string;
      body?: ProjectUserFilter
}

export function getProjectUsers(http: HttpClient, rootUrl: string, params: GetProjectUsers$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectUserPage>> {
  const rb = new RequestBuilder(rootUrl, getProjectUsers.PATH, 'post');
  if (params) {
    rb.path('projectId', params.projectId, {"style":"simple"});
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<ProjectUserPage>;
    })
  );
}

getProjectUsers.PATH = '/api/project/{projectId}/member/filter';
