/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ProjectPackageFilter } from '../../models/project-package-filter';
import { ProjectPackagePage } from '../../models/project-package-page';

export interface GetProjectPackages$Params {
  slug: string;
      body?: ProjectPackageFilter
}

export function getProjectPackages(http: HttpClient, rootUrl: string, params: GetProjectPackages$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectPackagePage>> {
  const rb = new RequestBuilder(rootUrl, getProjectPackages.PATH, 'post');
  if (params) {
    rb.path('slug', params.slug, {"style":"simple"});
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<ProjectPackagePage>;
    })
  );
}

getProjectPackages.PATH = '/api/project/{slug}/package/filter';