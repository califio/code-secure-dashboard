/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { EnvironmentVariable } from '../../models/environment-variable';

export interface SetProjectEnvironment$Params {
  projectId: string;
      body?: EnvironmentVariable
}

export function setProjectEnvironment(http: HttpClient, rootUrl: string, params: SetProjectEnvironment$Params, context?: HttpContext): Observable<StrictHttpResponse<EnvironmentVariable>> {
  const rb = new RequestBuilder(rootUrl, setProjectEnvironment.PATH, 'post');
  if (params) {
    rb.path('projectId', params.projectId, {"style":"simple"});
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<EnvironmentVariable>;
    })
  );
}

setProjectEnvironment.PATH = '/api/project/{projectId}/env';
