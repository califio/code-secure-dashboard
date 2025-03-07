/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ProjectPackageDetail } from '../../models/project-package-detail';

export interface GetProjectPackageDetail$Params {
  projectId: string;
  packageId: string;
}

export function getProjectPackageDetail(http: HttpClient, rootUrl: string, params: GetProjectPackageDetail$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectPackageDetail>> {
  const rb = new RequestBuilder(rootUrl, getProjectPackageDetail.PATH, 'get');
  if (params) {
    rb.path('projectId', params.projectId, {"style":"simple"});
    rb.path('packageId', params.packageId, {"style":"simple"});
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<ProjectPackageDetail>;
    })
  );
}

getProjectPackageDetail.PATH = '/api/project/{projectId}/package/{packageId}';
