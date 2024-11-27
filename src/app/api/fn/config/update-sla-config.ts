/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { SlaConfig } from '../../models/sla-config';

export interface UpdateSlaConfig$Params {
      body?: SlaConfig
}

export function updateSlaConfig(http: HttpClient, rootUrl: string, params?: UpdateSlaConfig$Params, context?: HttpContext): Observable<StrictHttpResponse<SlaConfig>> {
  const rb = new RequestBuilder(rootUrl, updateSlaConfig.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<SlaConfig>;
    })
  );
}

updateSlaConfig.PATH = '/api/config/sla';