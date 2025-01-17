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

import { createUserByAdmin } from '../fn/user/create-user-by-admin';
import { CreateUserByAdmin$Params } from '../fn/user/create-user-by-admin';
import { getUser } from '../fn/user/get-user';
import { GetUser$Params } from '../fn/user/get-user';
import { getUsers } from '../fn/user/get-users';
import { GetUsers$Params } from '../fn/user/get-users';
import { getUsersByAdmin } from '../fn/user/get-users-by-admin';
import { GetUsersByAdmin$Params } from '../fn/user/get-users-by-admin';
import { sendConfirmEmail } from '../fn/user/send-confirm-email';
import { SendConfirmEmail$Params } from '../fn/user/send-confirm-email';
import { updateUserByAdmin } from '../fn/user/update-user-by-admin';
import { UpdateUserByAdmin$Params } from '../fn/user/update-user-by-admin';
import { UserInfo } from '../models/user-info';
import { UserInfoPage } from '../models/user-info-page';
import { UserSummaryPage } from '../models/user-summary-page';

@Injectable({ providedIn: 'root' })
export class UserService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `getUsers()` */
  static readonly GetUsersPath = '/api/user/filter';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getUsers()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getUsers$Response(params?: GetUsers$Params, context?: HttpContext): Observable<StrictHttpResponse<UserSummaryPage>> {
    return getUsers(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getUsers$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getUsers(params?: GetUsers$Params, context?: HttpContext): Observable<UserSummaryPage> {
    return this.getUsers$Response(params, context).pipe(
      map((r: StrictHttpResponse<UserSummaryPage>): UserSummaryPage => r.body)
    );
  }

  /** Path part for operation `getUsersByAdmin()` */
  static readonly GetUsersByAdminPath = '/api/admin/user/filter';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getUsersByAdmin()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getUsersByAdmin$Response(params?: GetUsersByAdmin$Params, context?: HttpContext): Observable<StrictHttpResponse<UserInfoPage>> {
    return getUsersByAdmin(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getUsersByAdmin$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getUsersByAdmin(params?: GetUsersByAdmin$Params, context?: HttpContext): Observable<UserInfoPage> {
    return this.getUsersByAdmin$Response(params, context).pipe(
      map((r: StrictHttpResponse<UserInfoPage>): UserInfoPage => r.body)
    );
  }

  /** Path part for operation `getUser()` */
  static readonly GetUserPath = '/api/admin/user/{userId}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getUser()` instead.
   *
   * This method doesn't expect any request body.
   */
  getUser$Response(params: GetUser$Params, context?: HttpContext): Observable<StrictHttpResponse<UserInfo>> {
    return getUser(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getUser$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getUser(params: GetUser$Params, context?: HttpContext): Observable<UserInfo> {
    return this.getUser$Response(params, context).pipe(
      map((r: StrictHttpResponse<UserInfo>): UserInfo => r.body)
    );
  }

  /** Path part for operation `updateUserByAdmin()` */
  static readonly UpdateUserByAdminPath = '/api/admin/user/{userId}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateUserByAdmin()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateUserByAdmin$Response(params: UpdateUserByAdmin$Params, context?: HttpContext): Observable<StrictHttpResponse<UserInfo>> {
    return updateUserByAdmin(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateUserByAdmin$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateUserByAdmin(params: UpdateUserByAdmin$Params, context?: HttpContext): Observable<UserInfo> {
    return this.updateUserByAdmin$Response(params, context).pipe(
      map((r: StrictHttpResponse<UserInfo>): UserInfo => r.body)
    );
  }

  /** Path part for operation `createUserByAdmin()` */
  static readonly CreateUserByAdminPath = '/api/admin/user';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `createUserByAdmin()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  createUserByAdmin$Response(params?: CreateUserByAdmin$Params, context?: HttpContext): Observable<StrictHttpResponse<UserInfo>> {
    return createUserByAdmin(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `createUserByAdmin$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  createUserByAdmin(params?: CreateUserByAdmin$Params, context?: HttpContext): Observable<UserInfo> {
    return this.createUserByAdmin$Response(params, context).pipe(
      map((r: StrictHttpResponse<UserInfo>): UserInfo => r.body)
    );
  }

  /** Path part for operation `sendConfirmEmail()` */
  static readonly SendConfirmEmailPath = '/api/admin/user/{userId}/send-confirm-email';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `sendConfirmEmail()` instead.
   *
   * This method doesn't expect any request body.
   */
  sendConfirmEmail$Response(params: SendConfirmEmail$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return sendConfirmEmail(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `sendConfirmEmail$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  sendConfirmEmail(params: SendConfirmEmail$Params, context?: HttpContext): Observable<void> {
    return this.sendConfirmEmail$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

}