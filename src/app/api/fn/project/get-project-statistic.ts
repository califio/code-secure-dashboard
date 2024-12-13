/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ProjectStatistics } from '../../models/project-statistics';

export interface GetProjectStatistic$Params {
  projectId: string;
}

export function getProjectStatistic(http: HttpClient, rootUrl: string, params: GetProjectStatistic$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectStatistics>> {
  const rb = new RequestBuilder(rootUrl, getProjectStatistic.PATH, 'get');
  if (params) {
    rb.path('projectId', params.projectId, {"style":"simple"});
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<ProjectStatistics>;
    })
  );
}

getProjectStatistic.PATH = '/api/project/{projectId}/statistic';
