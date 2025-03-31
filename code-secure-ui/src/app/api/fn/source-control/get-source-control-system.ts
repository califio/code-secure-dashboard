/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { SourceControlSummary } from '../../models/source-control-summary';

export interface GetSourceControlSystem$Params {
}

export function getSourceControlSystem(http: HttpClient, rootUrl: string, params?: GetSourceControlSystem$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<SourceControlSummary>>> {
  const rb = new RequestBuilder(rootUrl, getSourceControlSystem.PATH, 'get');
  if (params) {
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<Array<SourceControlSummary>>;
    })
  );
}

getSourceControlSystem.PATH = '/api/source-control';
