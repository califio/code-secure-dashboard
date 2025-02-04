/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { JiraProject } from '../../models/jira-project';
import { JiraSetting } from '../../models/jira-setting';

export interface GetJiraProjects$Params {
  reload?: boolean;
      body?: JiraSetting
}

export function getJiraProjects(http: HttpClient, rootUrl: string, params?: GetJiraProjects$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<JiraProject>>> {
  const rb = new RequestBuilder(rootUrl, getJiraProjects.PATH, 'post');
  if (params) {
    rb.query('reload', params.reload, {"style":"form"});
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<Array<JiraProject>>;
    })
  );
}

getJiraProjects.PATH = '/api/integration/jira/projects';
