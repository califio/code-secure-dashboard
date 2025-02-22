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

import { getScanners } from '../fn/scanner/get-scanners';
import { GetScanners$Params } from '../fn/scanner/get-scanners';
import { Scanners } from '../models/scanners';

@Injectable({ providedIn: 'root' })
export class ScannerService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `getScanners()` */
  static readonly GetScannersPath = '/api/scanner';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getScanners()` instead.
   *
   * This method doesn't expect any request body.
   */
  getScanners$Response(params?: GetScanners$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<Scanners>>> {
    return getScanners(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getScanners$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getScanners(params?: GetScanners$Params, context?: HttpContext): Observable<Array<Scanners>> {
    return this.getScanners$Response(params, context).pipe(
      map((r: StrictHttpResponse<Array<Scanners>>): Array<Scanners> => r.body)
    );
  }

}
