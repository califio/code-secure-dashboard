/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ThresholdSetting } from '../../models/threshold-setting';

export interface UpdateSastSettingProject$Params {
  projectId: string;
      body?: ThresholdSetting
}

export function updateSastSettingProject(http: HttpClient, rootUrl: string, params: UpdateSastSettingProject$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
  const rb = new RequestBuilder(rootUrl, updateSastSettingProject.PATH, 'post');
  if (params) {
    rb.path('projectId', params.projectId, {"style":"simple"});
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'text', accept: '*/*', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
    })
  );
}

updateSastSettingProject.PATH = '/api/project/{projectId}/setting/sast';
