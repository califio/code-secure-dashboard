/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { RuleFilter } from '../../models/rule-filter';
import { RuleInfoPage } from '../../models/rule-info-page';

export interface GetRuleByFilter$Params {
      body?: RuleFilter
}

export function getRuleByFilter(http: HttpClient, rootUrl: string, params?: GetRuleByFilter$Params, context?: HttpContext): Observable<StrictHttpResponse<RuleInfoPage>> {
  const rb = new RequestBuilder(rootUrl, getRuleByFilter.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<RuleInfoPage>;
    })
  );
}

getRuleByFilter.PATH = '/api/rule/filter';
