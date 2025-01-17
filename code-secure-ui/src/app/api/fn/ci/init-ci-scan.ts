/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { CiScanInfo } from '../../models/ci-scan-info';
import { CiScanRequest } from '../../models/ci-scan-request';

export interface InitCiScan$Params {
      body?: CiScanRequest
}

export function initCiScan(http: HttpClient, rootUrl: string, params?: InitCiScan$Params, context?: HttpContext): Observable<StrictHttpResponse<CiScanInfo>> {
  const rb = new RequestBuilder(rootUrl, initCiScan.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<CiScanInfo>;
    })
  );
}

initCiScan.PATH = '/api/ci/scan';