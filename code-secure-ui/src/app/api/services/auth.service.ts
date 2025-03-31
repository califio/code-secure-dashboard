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
import { confirmEmail } from '../fn/auth/confirm-email';
import { ConfirmEmail$Params } from '../fn/auth/confirm-email';
import { ConfirmEmailResponse } from '../models/confirm-email-response';
import { forgotPassword } from '../fn/auth/forgot-password';
import { ForgotPassword$Params } from '../fn/auth/forgot-password';
import { getAuthConfig } from '../fn/auth/get-auth-config';
import { GetAuthConfig$Params } from '../fn/auth/get-auth-config';
import { login } from '../fn/auth/login';
import { Login$Params } from '../fn/auth/login';
import { logout } from '../fn/auth/logout';
import { Logout$Params } from '../fn/auth/logout';
import { refreshToken } from '../fn/auth/refresh-token';
import { RefreshToken$Params } from '../fn/auth/refresh-token';
import { renderMail } from '../fn/auth/render-mail';
import { RenderMail$Params } from '../fn/auth/render-mail';
import { resetPassword } from '../fn/auth/reset-password';
import { ResetPassword$Params } from '../fn/auth/reset-password';
import { SignInResponse } from '../models/sign-in-response';

@Injectable({ providedIn: 'root' })
export class AuthService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `renderMail()` */
  static readonly RenderMailPath = '/api/render-mail';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `renderMail()` instead.
   *
   * This method doesn't expect any request body.
   */
  renderMail$Response(params?: RenderMail$Params, context?: HttpContext): Observable<StrictHttpResponse<string>> {
    return renderMail(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `renderMail$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  renderMail(params?: RenderMail$Params, context?: HttpContext): Observable<string> {
    return this.renderMail$Response(params, context).pipe(
      map((r: StrictHttpResponse<string>): string => r.body)
    );
  }

  /** Path part for operation `getAuthConfig()` */
  static readonly GetAuthConfigPath = '/api/auth-config';

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

  /** Path part for operation `login()` */
  static readonly LoginPath = '/api/login';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `login()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  login$Response(params?: Login$Params, context?: HttpContext): Observable<StrictHttpResponse<SignInResponse>> {
    return login(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `login$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  login(params?: Login$Params, context?: HttpContext): Observable<SignInResponse> {
    return this.login$Response(params, context).pipe(
      map((r: StrictHttpResponse<SignInResponse>): SignInResponse => r.body)
    );
  }

  /** Path part for operation `refreshToken()` */
  static readonly RefreshTokenPath = '/api/refresh-token';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `refreshToken()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  refreshToken$Response(params?: RefreshToken$Params, context?: HttpContext): Observable<StrictHttpResponse<SignInResponse>> {
    return refreshToken(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `refreshToken$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  refreshToken(params?: RefreshToken$Params, context?: HttpContext): Observable<SignInResponse> {
    return this.refreshToken$Response(params, context).pipe(
      map((r: StrictHttpResponse<SignInResponse>): SignInResponse => r.body)
    );
  }

  /** Path part for operation `logout()` */
  static readonly LogoutPath = '/api/logout';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `logout()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  logout$Response(params?: Logout$Params, context?: HttpContext): Observable<StrictHttpResponse<boolean>> {
    return logout(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `logout$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  logout(params?: Logout$Params, context?: HttpContext): Observable<boolean> {
    return this.logout$Response(params, context).pipe(
      map((r: StrictHttpResponse<boolean>): boolean => r.body)
    );
  }

  /** Path part for operation `forgotPassword()` */
  static readonly ForgotPasswordPath = '/api/forgot-password';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `forgotPassword()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  forgotPassword$Response(params?: ForgotPassword$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return forgotPassword(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `forgotPassword$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  forgotPassword(params?: ForgotPassword$Params, context?: HttpContext): Observable<void> {
    return this.forgotPassword$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `resetPassword()` */
  static readonly ResetPasswordPath = '/api/reset-password';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `resetPassword()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  resetPassword$Response(params?: ResetPassword$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return resetPassword(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `resetPassword$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  resetPassword(params?: ResetPassword$Params, context?: HttpContext): Observable<void> {
    return this.resetPassword$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `confirmEmail()` */
  static readonly ConfirmEmailPath = '/api/confirm-email';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `confirmEmail()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  confirmEmail$Response(params?: ConfirmEmail$Params, context?: HttpContext): Observable<StrictHttpResponse<ConfirmEmailResponse>> {
    return confirmEmail(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `confirmEmail$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  confirmEmail(params?: ConfirmEmail$Params, context?: HttpContext): Observable<ConfirmEmailResponse> {
    return this.confirmEmail$Response(params, context).pipe(
      map((r: StrictHttpResponse<ConfirmEmailResponse>): ConfirmEmailResponse => r.body)
    );
  }

}
