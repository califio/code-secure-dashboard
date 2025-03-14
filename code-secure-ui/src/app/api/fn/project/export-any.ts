/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ExportType } from '../../models/export-type';
import { ProjectFindingFilter } from '../../models/project-finding-filter';

export interface Export$Any$Params {
  format?: ExportType;
  projectId: string;
      body?: ProjectFindingFilter
}

export function export$Any(http: HttpClient, rootUrl: string, params: Export$Any$Params, context?: HttpContext): Observable<StrictHttpResponse<Blob>> {
  const rb = new RequestBuilder(rootUrl, export$Any.PATH, 'post');
  if (params) {
    rb.query('format', params.format, {"style":"form"});
    rb.path('projectId', params.projectId, {"style":"simple"});
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'blob', accept: 'application/octet-stream', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<Blob>;
    })
  );
}

export$Any.PATH = '/api/project/{projectId}/export';
