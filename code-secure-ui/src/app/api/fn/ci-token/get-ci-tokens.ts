/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { CiTokens } from '../../models/ci-tokens';

export interface GetCiTokens$Params {
}

export function getCiTokens(http: HttpClient, rootUrl: string, params?: GetCiTokens$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<CiTokens>>> {
  const rb = new RequestBuilder(rootUrl, getCiTokens.PATH, 'get');
  if (params) {
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<Array<CiTokens>>;
    })
  );
}

getCiTokens.PATH = '/api/admin/ci-token';