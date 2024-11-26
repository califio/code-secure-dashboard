/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { SastStatistic } from '../../models/sast-statistic';

export interface SastStatistic$Params {
}

export function sastStatistic(http: HttpClient, rootUrl: string, params?: SastStatistic$Params, context?: HttpContext): Observable<StrictHttpResponse<SastStatistic>> {
  const rb = new RequestBuilder(rootUrl, sastStatistic.PATH, 'get');
  if (params) {
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<SastStatistic>;
    })
  );
}

sastStatistic.PATH = '/api/dashboard/sast';
