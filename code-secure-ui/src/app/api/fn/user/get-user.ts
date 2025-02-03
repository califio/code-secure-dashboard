/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { UserInfo } from '../../models/user-info';

export interface GetUser$Params {
  userId: string;
}

export function getUser(http: HttpClient, rootUrl: string, params: GetUser$Params, context?: HttpContext): Observable<StrictHttpResponse<UserInfo>> {
  const rb = new RequestBuilder(rootUrl, getUser.PATH, 'get');
  if (params) {
    rb.path('userId', params.userId, {"style":"simple"});
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<UserInfo>;
    })
  );
}

getUser.PATH = '/api/user/{userId}';
