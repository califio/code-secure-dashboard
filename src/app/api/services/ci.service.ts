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

import { initCiScan } from '../fn/ci/init-ci-scan';
import { InitCiScan$Params } from '../fn/ci/init-ci-scan';
import { updateCiScan } from '../fn/ci/update-ci-scan';
import { UpdateCiScan$Params } from '../fn/ci/update-ci-scan';
import { uploadSastFinding } from '../fn/ci/upload-sast-finding';
import { UploadSastFinding$Params } from '../fn/ci/upload-sast-finding';
import { UploadSastFindingResponse } from '../models/upload-sast-finding-response';

@Injectable({ providedIn: 'root' })
export class CiService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `initCiScan()` */
  static readonly InitCiScanPath = '/api/ci/scan';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `initCiScan()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  initCiScan$Response(params?: InitCiScan$Params, context?: HttpContext): Observable<StrictHttpResponse<string>> {
    return initCiScan(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `initCiScan$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  initCiScan(params?: InitCiScan$Params, context?: HttpContext): Observable<string> {
    return this.initCiScan$Response(params, context).pipe(
      map((r: StrictHttpResponse<string>): string => r.body)
    );
  }

  /** Path part for operation `updateCiScan()` */
  static readonly UpdateCiScanPath = '/api/ci/scan/{scanId}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateCiScan()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateCiScan$Response(params: UpdateCiScan$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return updateCiScan(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateCiScan$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateCiScan(params: UpdateCiScan$Params, context?: HttpContext): Observable<void> {
    return this.updateCiScan$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `uploadSastFinding()` */
  static readonly UploadSastFindingPath = '/api/ci/sast-finding';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `uploadSastFinding()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  uploadSastFinding$Response(params?: UploadSastFinding$Params, context?: HttpContext): Observable<StrictHttpResponse<UploadSastFindingResponse>> {
    return uploadSastFinding(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `uploadSastFinding$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  uploadSastFinding(params?: UploadSastFinding$Params, context?: HttpContext): Observable<UploadSastFindingResponse> {
    return this.uploadSastFinding$Response(params, context).pipe(
      map((r: StrictHttpResponse<UploadSastFindingResponse>): UploadSastFindingResponse => r.body)
    );
  }

}
