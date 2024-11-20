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

import { AuthConfig } from '../models/auth-config';
import { getAuthConfig } from '../fn/config/get-auth-config';
import { GetAuthConfig$Params } from '../fn/config/get-auth-config';
import { getMailConfig } from '../fn/config/get-mail-config';
import { GetMailConfig$Params } from '../fn/config/get-mail-config';
import { getOidcConfig } from '../fn/config/get-oidc-config';
import { GetOidcConfig$Params } from '../fn/config/get-oidc-config';
import { MailConfig } from '../models/mail-config';
import { OidcConfig } from '../models/oidc-config';
import { updateAuthConfig } from '../fn/config/update-auth-config';
import { UpdateAuthConfig$Params } from '../fn/config/update-auth-config';
import { updateMailConfig } from '../fn/config/update-mail-config';
import { UpdateMailConfig$Params } from '../fn/config/update-mail-config';

@Injectable({ providedIn: 'root' })
export class ConfigService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `getOidcConfig()` */
  static readonly GetOidcConfigPath = '/api/config/oidc';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getOidcConfig()` instead.
   *
   * This method doesn't expect any request body.
   */
  getOidcConfig$Response(params?: GetOidcConfig$Params, context?: HttpContext): Observable<StrictHttpResponse<OidcConfig>> {
    return getOidcConfig(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getOidcConfig$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getOidcConfig(params?: GetOidcConfig$Params, context?: HttpContext): Observable<OidcConfig> {
    return this.getOidcConfig$Response(params, context).pipe(
      map((r: StrictHttpResponse<OidcConfig>): OidcConfig => r.body)
    );
  }

  /** Path part for operation `getMailConfig()` */
  static readonly GetMailConfigPath = '/api/config/mail';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getMailConfig()` instead.
   *
   * This method doesn't expect any request body.
   */
  getMailConfig$Response(params?: GetMailConfig$Params, context?: HttpContext): Observable<StrictHttpResponse<MailConfig>> {
    return getMailConfig(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getMailConfig$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getMailConfig(params?: GetMailConfig$Params, context?: HttpContext): Observable<MailConfig> {
    return this.getMailConfig$Response(params, context).pipe(
      map((r: StrictHttpResponse<MailConfig>): MailConfig => r.body)
    );
  }

  /** Path part for operation `updateMailConfig()` */
  static readonly UpdateMailConfigPath = '/api/config/mail';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateMailConfig()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateMailConfig$Response(params?: UpdateMailConfig$Params, context?: HttpContext): Observable<StrictHttpResponse<MailConfig>> {
    return updateMailConfig(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateMailConfig$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateMailConfig(params?: UpdateMailConfig$Params, context?: HttpContext): Observable<MailConfig> {
    return this.updateMailConfig$Response(params, context).pipe(
      map((r: StrictHttpResponse<MailConfig>): MailConfig => r.body)
    );
  }

  /** Path part for operation `getAuthConfig()` */
  static readonly GetAuthConfigPath = '/api/config/auth';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getAuthConfig()` instead.
   *
   * This method doesn't expect any request body.
   */
  getAuthConfig$Response(params?: GetAuthConfig$Params, context?: HttpContext): Observable<StrictHttpResponse<AuthConfig>> {
    return getAuthConfig(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getAuthConfig$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getAuthConfig(params?: GetAuthConfig$Params, context?: HttpContext): Observable<AuthConfig> {
    return this.getAuthConfig$Response(params, context).pipe(
      map((r: StrictHttpResponse<AuthConfig>): AuthConfig => r.body)
    );
  }

  /** Path part for operation `updateAuthConfig()` */
  static readonly UpdateAuthConfigPath = '/api/config/auth';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateAuthConfig()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateAuthConfig$Response(params?: UpdateAuthConfig$Params, context?: HttpContext): Observable<StrictHttpResponse<AuthConfig>> {
    return updateAuthConfig(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateAuthConfig$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateAuthConfig(params?: UpdateAuthConfig$Params, context?: HttpContext): Observable<AuthConfig> {
    return this.updateAuthConfig$Response(params, context).pipe(
      map((r: StrictHttpResponse<AuthConfig>): AuthConfig => r.body)
    );
  }

}