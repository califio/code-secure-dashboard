/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { CreateProjectMemberRequest } from '../../models/create-project-member-request';
import { ProjectMember } from '../../models/project-member';

export interface AddMember$Params {
  projectId: string;
      body?: CreateProjectMemberRequest
}

export function addMember(http: HttpClient, rootUrl: string, params: AddMember$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectMember>> {
  const rb = new RequestBuilder(rootUrl, addMember.PATH, 'post');
  if (params) {
    rb.path('projectId', params.projectId, {"style":"simple"});
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<ProjectMember>;
    })
  );
}

addMember.PATH = '/api/project/{projectId}/member';
