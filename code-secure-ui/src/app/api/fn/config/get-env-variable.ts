/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { QueryFilter } from '../../models/query-filter';
import { StringPage } from '../../models/string-page';

export interface GetEnvVariable$Params {
      body?: QueryFilter
}

export function getEnvVariable(http: HttpClient, rootUrl: string, params?: GetEnvVariable$Params, context?: HttpContext): Observable<StrictHttpResponse<StringPage>> {
  const rb = new RequestBuilder(rootUrl, getEnvVariable.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<StringPage>;
    })
  );
}

getEnvVariable.PATH = '/api/config/env/filter';
