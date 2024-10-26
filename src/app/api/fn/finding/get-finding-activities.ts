/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { FindingActivityPage } from '../../models/finding-activity-page';
import { QueryFilter } from '../../models/query-filter';

export interface GetFindingActivities$Params {
  sid: string;
      body?: QueryFilter
}

export function getFindingActivities(http: HttpClient, rootUrl: string, params: GetFindingActivities$Params, context?: HttpContext): Observable<StrictHttpResponse<FindingActivityPage>> {
  const rb = new RequestBuilder(rootUrl, getFindingActivities.PATH, 'get');
  if (params) {
    rb.path('sid', params.sid, {"style":"simple"});
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<FindingActivityPage>;
    })
  );
}

getFindingActivities.PATH = '/api/finding/{sid}/activity';
