/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { EnvironmentVariable } from '../../models/environment-variable';

export interface GetScanEnvironmentVariables$Params {
  scanId: string;
}

export function getScanEnvironmentVariables(http: HttpClient, rootUrl: string, params: GetScanEnvironmentVariables$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<EnvironmentVariable>>> {
  const rb = new RequestBuilder(rootUrl, getScanEnvironmentVariables.PATH, 'get');
  if (params) {
    rb.path('scanId', params.scanId, {"style":"simple"});
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<Array<EnvironmentVariable>>;
    })
  );
}

getScanEnvironmentVariables.PATH = '/api/ci/scan/{scanId}/env';
