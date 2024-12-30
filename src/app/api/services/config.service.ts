/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';

import { AuthInfo } from '../models/auth-info';
import { AuthSetting } from '../models/auth-setting';
import { createEnvVariable } from '../fn/config/create-env-variable';
import { CreateEnvVariable$Params } from '../fn/config/create-env-variable';
import { deleteEnvVariable } from '../fn/config/delete-env-variable';
import { DeleteEnvVariable$Params } from '../fn/config/delete-env-variable';
import { getAuthInfo } from '../fn/config/get-auth-info';
import { GetAuthInfo$Params } from '../fn/config/get-auth-info';
import { getAuthSetting } from '../fn/config/get-auth-setting';
import { GetAuthSetting$Params } from '../fn/config/get-auth-setting';
import { getEnvVariable } from '../fn/config/get-env-variable';
import { GetEnvVariable$Params } from '../fn/config/get-env-variable';
import { getMailSetting } from '../fn/config/get-mail-setting';
import { GetMailSetting$Params } from '../fn/config/get-mail-setting';
import { getSlaSetting } from '../fn/config/get-sla-setting';
import { GetSlaSetting$Params } from '../fn/config/get-sla-setting';
import { getTeamsSetting } from '../fn/config/get-teams-setting';
import { GetTeamsSetting$Params } from '../fn/config/get-teams-setting';
import { MailSetting } from '../models/mail-setting';
import { SlaSetting } from '../models/sla-setting';
import { StringPage } from '../models/string-page';
import { TeamsNotificationSetting } from '../models/teams-notification-setting';
import { testMailSetting } from '../fn/config/test-mail-setting';
import { TestMailSetting$Params } from '../fn/config/test-mail-setting';
import { testTeamsSetting } from '../fn/config/test-teams-setting';
import { TestTeamsSetting$Params } from '../fn/config/test-teams-setting';
import { updateAuthSetting } from '../fn/config/update-auth-setting';
import { UpdateAuthSetting$Params } from '../fn/config/update-auth-setting';
import { updateMailSetting } from '../fn/config/update-mail-setting';
import { UpdateMailSetting$Params } from '../fn/config/update-mail-setting';
import { updateSlaSetting } from '../fn/config/update-sla-setting';
import { UpdateSlaSetting$Params } from '../fn/config/update-sla-setting';
import { updateTeamsSetting } from '../fn/config/update-teams-setting';
import { UpdateTeamsSetting$Params } from '../fn/config/update-teams-setting';

@Injectable({ providedIn: 'root' })
export class ConfigService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `getAuthInfo()` */
  static readonly GetAuthInfoPath = '/api/config/authInfo';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getAuthInfo()` instead.
   *
   * This method doesn't expect any request body.
   */
  getAuthInfo$Response(params?: GetAuthInfo$Params, context?: HttpContext): Observable<StrictHttpResponse<AuthInfo>> {
    return getAuthInfo(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getAuthInfo$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getAuthInfo(params?: GetAuthInfo$Params, context?: HttpContext): Observable<AuthInfo> {
    return this.getAuthInfo$Response(params, context).pipe(
      map((r: StrictHttpResponse<AuthInfo>): AuthInfo => r.body)
    );
  }

  /** Path part for operation `getMailSetting()` */
  static readonly GetMailSettingPath = '/api/config/mail';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getMailSetting()` instead.
   *
   * This method doesn't expect any request body.
   */
  getMailSetting$Response(params?: GetMailSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<MailSetting>> {
    return getMailSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getMailSetting$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getMailSetting(params?: GetMailSetting$Params, context?: HttpContext): Observable<MailSetting> {
    return this.getMailSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<MailSetting>): MailSetting => r.body)
    );
  }

  /** Path part for operation `updateMailSetting()` */
  static readonly UpdateMailSettingPath = '/api/config/mail';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateMailSetting()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateMailSetting$Response(params?: UpdateMailSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<MailSetting>> {
    return updateMailSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateMailSetting$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateMailSetting(params?: UpdateMailSetting$Params, context?: HttpContext): Observable<MailSetting> {
    return this.updateMailSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<MailSetting>): MailSetting => r.body)
    );
  }

  /** Path part for operation `testMailSetting()` */
  static readonly TestMailSettingPath = '/api/config/mail/test';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `testMailSetting()` instead.
   *
   * This method doesn't expect any request body.
   */
  testMailSetting$Response(params?: TestMailSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<string>> {
    return testMailSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `testMailSetting$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  testMailSetting(params?: TestMailSetting$Params, context?: HttpContext): Observable<string> {
    return this.testMailSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<string>): string => r.body)
    );
  }

  /** Path part for operation `getTeamsSetting()` */
  static readonly GetTeamsSettingPath = '/api/config/teams';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getTeamsSetting()` instead.
   *
   * This method doesn't expect any request body.
   */
  getTeamsSetting$Response(params?: GetTeamsSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<TeamsNotificationSetting>> {
    return getTeamsSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getTeamsSetting$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getTeamsSetting(params?: GetTeamsSetting$Params, context?: HttpContext): Observable<TeamsNotificationSetting> {
    return this.getTeamsSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<TeamsNotificationSetting>): TeamsNotificationSetting => r.body)
    );
  }

  /** Path part for operation `updateTeamsSetting()` */
  static readonly UpdateTeamsSettingPath = '/api/config/teams';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateTeamsSetting()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateTeamsSetting$Response(params?: UpdateTeamsSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<TeamsNotificationSetting>> {
    return updateTeamsSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateTeamsSetting$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateTeamsSetting(params?: UpdateTeamsSetting$Params, context?: HttpContext): Observable<TeamsNotificationSetting> {
    return this.updateTeamsSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<TeamsNotificationSetting>): TeamsNotificationSetting => r.body)
    );
  }

  /** Path part for operation `testTeamsSetting()` */
  static readonly TestTeamsSettingPath = '/api/config/teams/test';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `testTeamsSetting()` instead.
   *
   * This method doesn't expect any request body.
   */
  testTeamsSetting$Response(params?: TestTeamsSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return testTeamsSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `testTeamsSetting$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  testTeamsSetting(params?: TestTeamsSetting$Params, context?: HttpContext): Observable<void> {
    return this.testTeamsSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `getAuthSetting()` */
  static readonly GetAuthSettingPath = '/api/config/auth';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getAuthSetting()` instead.
   *
   * This method doesn't expect any request body.
   */
  getAuthSetting$Response(params?: GetAuthSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<AuthSetting>> {
    return getAuthSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getAuthSetting$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getAuthSetting(params?: GetAuthSetting$Params, context?: HttpContext): Observable<AuthSetting> {
    return this.getAuthSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<AuthSetting>): AuthSetting => r.body)
    );
  }

  /** Path part for operation `updateAuthSetting()` */
  static readonly UpdateAuthSettingPath = '/api/config/auth';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateAuthSetting()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateAuthSetting$Response(params?: UpdateAuthSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<AuthSetting>> {
    return updateAuthSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateAuthSetting$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateAuthSetting(params?: UpdateAuthSetting$Params, context?: HttpContext): Observable<AuthSetting> {
    return this.updateAuthSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<AuthSetting>): AuthSetting => r.body)
    );
  }

  /** Path part for operation `getSlaSetting()` */
  static readonly GetSlaSettingPath = '/api/config/sla';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getSlaSetting()` instead.
   *
   * This method doesn't expect any request body.
   */
  getSlaSetting$Response(params?: GetSlaSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<SlaSetting>> {
    return getSlaSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getSlaSetting$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getSlaSetting(params?: GetSlaSetting$Params, context?: HttpContext): Observable<SlaSetting> {
    return this.getSlaSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<SlaSetting>): SlaSetting => r.body)
    );
  }

  /** Path part for operation `updateSlaSetting()` */
  static readonly UpdateSlaSettingPath = '/api/config/sla';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateSlaSetting()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateSlaSetting$Response(params?: UpdateSlaSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<SlaSetting>> {
    return updateSlaSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateSlaSetting$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateSlaSetting(params?: UpdateSlaSetting$Params, context?: HttpContext): Observable<SlaSetting> {
    return this.updateSlaSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<SlaSetting>): SlaSetting => r.body)
    );
  }

  /** Path part for operation `getEnvVariable()` */
  static readonly GetEnvVariablePath = '/api/config/env/filter';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getEnvVariable()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getEnvVariable$Response(params?: GetEnvVariable$Params, context?: HttpContext): Observable<StrictHttpResponse<StringPage>> {
    return getEnvVariable(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getEnvVariable$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getEnvVariable(params?: GetEnvVariable$Params, context?: HttpContext): Observable<StringPage> {
    return this.getEnvVariable$Response(params, context).pipe(
      map((r: StrictHttpResponse<StringPage>): StringPage => r.body)
    );
  }

  /** Path part for operation `createEnvVariable()` */
  static readonly CreateEnvVariablePath = '/api/config/env';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `createEnvVariable()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  createEnvVariable$Response(params?: CreateEnvVariable$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return createEnvVariable(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `createEnvVariable$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  createEnvVariable(params?: CreateEnvVariable$Params, context?: HttpContext): Observable<void> {
    return this.createEnvVariable$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `deleteEnvVariable()` */
  static readonly DeleteEnvVariablePath = '/api/config/env/{env}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `deleteEnvVariable()` instead.
   *
   * This method doesn't expect any request body.
   */
  deleteEnvVariable$Response(params: DeleteEnvVariable$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return deleteEnvVariable(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `deleteEnvVariable$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  deleteEnvVariable(params: DeleteEnvVariable$Params, context?: HttpContext): Observable<void> {
    return this.deleteEnvVariable$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

}
