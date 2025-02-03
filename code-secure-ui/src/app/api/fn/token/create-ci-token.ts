/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { CiTokens } from '../../models/ci-tokens';
import { CreateTokenRequest } from '../../models/create-token-request';

export interface CreateCiToken$Params {
      body?: CreateTokenRequest
}

export function createCiToken(http: HttpClient, rootUrl: string, params?: CreateCiToken$Params, context?: HttpContext): Observable<StrictHttpResponse<CiTokens>> {
  const rb = new RequestBuilder(rootUrl, createCiToken.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<CiTokens>;
    })
  );
}

createCiToken.PATH = '/api/token';
