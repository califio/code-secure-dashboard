/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ProjectSortField } from '../../models/project-sort-field';
import { ProjectSummaryPage } from '../../models/project-summary-page';

export interface GetProjects$Params {
  Name?: string;
  SortBy?: ProjectSortField;
  Size?: number;
  Page?: number;
  Desc?: boolean;
}

export function getProjects(http: HttpClient, rootUrl: string, params?: GetProjects$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectSummaryPage>> {
  const rb = new RequestBuilder(rootUrl, getProjects.PATH, 'get');
  if (params) {
    rb.query('Name', params.Name, {"style":"form"});
    rb.query('SortBy', params.SortBy, {"style":"form"});
    rb.query('Size', params.Size, {"style":"form"});
    rb.query('Page', params.Page, {"style":"form"});
    rb.query('Desc', params.Desc, {"style":"form"});
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<ProjectSummaryPage>;
    })
  );
}

getProjects.PATH = '/api/project';
