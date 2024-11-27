/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ProjectSettingMetadata } from '../../models/project-setting-metadata';

export interface GetProjectSetting$Params {
  slug: string;
}

export function getProjectSetting(http: HttpClient, rootUrl: string, params: GetProjectSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectSettingMetadata>> {
  const rb = new RequestBuilder(rootUrl, getProjectSetting.PATH, 'get');
  if (params) {
    rb.path('slug', params.slug, {"style":"simple"});
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<ProjectSettingMetadata>;
    })
  );
}

getProjectSetting.PATH = '/api/project/{slug}/setting';