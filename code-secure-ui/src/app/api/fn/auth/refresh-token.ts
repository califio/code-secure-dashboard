/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { RefreshTokenRequest } from '../../models/refresh-token-request';
import { SignInResponse } from '../../models/sign-in-response';

export interface RefreshToken$Params {
      body?: RefreshTokenRequest
}

export function refreshToken(http: HttpClient, rootUrl: string, params?: RefreshToken$Params, context?: HttpContext): Observable<StrictHttpResponse<SignInResponse>> {
  const rb = new RequestBuilder(rootUrl, refreshToken.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<SignInResponse>;
    })
  );
}

refreshToken.PATH = '/api/refresh-token';
