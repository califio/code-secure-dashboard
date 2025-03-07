/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { Scanners } from '../../models/scanners';

export interface GetSastScanners$Params {
  projectId?: string;
}

export function getSastScanners(http: HttpClient, rootUrl: string, params?: GetSastScanners$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<Scanners>>> {
  const rb = new RequestBuilder(rootUrl, getSastScanners.PATH, 'get');
  if (params) {
    rb.query('projectId', params.projectId, {"style":"form"});
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<Array<Scanners>>;
    })
  );
}

getSastScanners.PATH = '/api/scanner/sast';
