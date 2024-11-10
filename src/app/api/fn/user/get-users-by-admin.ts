/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { UserFilter } from '../../models/user-filter';
import { UserInfoPage } from '../../models/user-info-page';

export interface GetUsersByAdmin$Params {
      body?: UserFilter
}

export function getUsersByAdmin(http: HttpClient, rootUrl: string, params?: GetUsersByAdmin$Params, context?: HttpContext): Observable<StrictHttpResponse<UserInfoPage>> {
  const rb = new RequestBuilder(rootUrl, getUsersByAdmin.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<UserInfoPage>;
    })
  );
}

getUsersByAdmin.PATH = '/api/admin/user/filter';
