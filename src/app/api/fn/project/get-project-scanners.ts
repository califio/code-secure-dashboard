/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ProjectScanner } from '../../models/project-scanner';

export interface GetProjectScanners$Params {
  slug: string;
}

export function getProjectScanners(http: HttpClient, rootUrl: string, params: GetProjectScanners$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<ProjectScanner>>> {
  const rb = new RequestBuilder(rootUrl, getProjectScanners.PATH, 'get');
  if (params) {
    rb.path('slug', params.slug, {"style":"simple"});
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<Array<ProjectScanner>>;
    })
  );
}

getProjectScanners.PATH = '/api/project/{slug}/scanner';