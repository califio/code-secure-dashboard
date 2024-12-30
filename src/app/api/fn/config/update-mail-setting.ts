/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { MailSetting } from '../../models/mail-setting';
import { MailSettingRequest } from '../../models/mail-setting-request';

export interface UpdateMailSetting$Params {
      body?: MailSettingRequest
}

export function updateMailSetting(http: HttpClient, rootUrl: string, params?: UpdateMailSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<MailSetting>> {
  const rb = new RequestBuilder(rootUrl, updateMailSetting.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<MailSetting>;
    })
  );
}

updateMailSetting.PATH = '/api/config/mail';
