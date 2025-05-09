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

import { addComment } from '../fn/finding/add-comment';
import { AddComment$Params } from '../fn/finding/add-comment';
import { createTicket } from '../fn/finding/create-ticket';
import { CreateTicket$Params } from '../fn/finding/create-ticket';
import { deleteTicket } from '../fn/finding/delete-ticket';
import { DeleteTicket$Params } from '../fn/finding/delete-ticket';
import { exportFinding$Any } from '../fn/finding/export-finding-any';
import { ExportFinding$Any$Params } from '../fn/finding/export-finding-any';
import { exportFinding$Json } from '../fn/finding/export-finding-json';
import { ExportFinding$Json$Params } from '../fn/finding/export-finding-json';
import { FindingActivity } from '../models/finding-activity';
import { FindingActivityPage } from '../models/finding-activity-page';
import { FindingDetail } from '../models/finding-detail';
import { FindingStatus } from '../models/finding-status';
import { FindingSummaryPage } from '../models/finding-summary-page';
import { getFindingActivities } from '../fn/finding/get-finding-activities';
import { GetFindingActivities$Params } from '../fn/finding/get-finding-activities';
import { getFindingById } from '../fn/finding/get-finding-by-id';
import { GetFindingById$Params } from '../fn/finding/get-finding-by-id';
import { getFindingRules } from '../fn/finding/get-finding-rules';
import { GetFindingRules$Params } from '../fn/finding/get-finding-rules';
import { getFindings } from '../fn/finding/get-findings';
import { GetFindings$Params } from '../fn/finding/get-findings';
import { listFindingCategory } from '../fn/finding/list-finding-category';
import { ListFindingCategory$Params } from '../fn/finding/list-finding-category';
import { Tickets } from '../models/tickets';
import { updateFinding } from '../fn/finding/update-finding';
import { UpdateFinding$Params } from '../fn/finding/update-finding';
import { updateStatusScanFinding } from '../fn/finding/update-status-scan-finding';
import { UpdateStatusScanFinding$Params } from '../fn/finding/update-status-scan-finding';

@Injectable({ providedIn: 'root' })
export class FindingService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `getFindings()` */
  static readonly GetFindingsPath = '/api/finding/filter';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getFindings()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getFindings$Response(params?: GetFindings$Params, context?: HttpContext): Observable<StrictHttpResponse<FindingSummaryPage>> {
    return getFindings(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getFindings$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getFindings(params?: GetFindings$Params, context?: HttpContext): Observable<FindingSummaryPage> {
    return this.getFindings$Response(params, context).pipe(
      map((r: StrictHttpResponse<FindingSummaryPage>): FindingSummaryPage => r.body)
    );
  }

  /** Path part for operation `exportFinding()` */
  static readonly ExportFindingPath = '/api/finding/export';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `exportFinding$Any()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  exportFinding$Any$Response(params?: ExportFinding$Any$Params, context?: HttpContext): Observable<StrictHttpResponse<Blob>> {
    return exportFinding$Any(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `exportFinding$Any$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  exportFinding$Any(params?: ExportFinding$Any$Params, context?: HttpContext): Observable<Blob> {
    return this.exportFinding$Any$Response(params, context).pipe(
      map((r: StrictHttpResponse<Blob>): Blob => r.body)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `exportFinding$Json()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  exportFinding$Json$Response(params?: ExportFinding$Json$Params, context?: HttpContext): Observable<StrictHttpResponse<Blob>> {
    return exportFinding$Json(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `exportFinding$Json$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  exportFinding$Json(params?: ExportFinding$Json$Params, context?: HttpContext): Observable<Blob> {
    return this.exportFinding$Json$Response(params, context).pipe(
      map((r: StrictHttpResponse<Blob>): Blob => r.body)
    );
  }

  /** Path part for operation `getFindingById()` */
  static readonly GetFindingByIdPath = '/api/finding/{findingId}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getFindingById()` instead.
   *
   * This method doesn't expect any request body.
   */
  getFindingById$Response(params: GetFindingById$Params, context?: HttpContext): Observable<StrictHttpResponse<FindingDetail>> {
    return getFindingById(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getFindingById$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getFindingById(params: GetFindingById$Params, context?: HttpContext): Observable<FindingDetail> {
    return this.getFindingById$Response(params, context).pipe(
      map((r: StrictHttpResponse<FindingDetail>): FindingDetail => r.body)
    );
  }

  /** Path part for operation `updateFinding()` */
  static readonly UpdateFindingPath = '/api/finding/{findingId}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateFinding()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateFinding$Response(params: UpdateFinding$Params, context?: HttpContext): Observable<StrictHttpResponse<FindingDetail>> {
    return updateFinding(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateFinding$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateFinding(params: UpdateFinding$Params, context?: HttpContext): Observable<FindingDetail> {
    return this.updateFinding$Response(params, context).pipe(
      map((r: StrictHttpResponse<FindingDetail>): FindingDetail => r.body)
    );
  }

  /** Path part for operation `updateStatusScanFinding()` */
  static readonly UpdateStatusScanFindingPath = '/api/finding/{findingId}/scan-status';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateStatusScanFinding()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateStatusScanFinding$Response(params: UpdateStatusScanFinding$Params, context?: HttpContext): Observable<StrictHttpResponse<FindingStatus>> {
    return updateStatusScanFinding(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateStatusScanFinding$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateStatusScanFinding(params: UpdateStatusScanFinding$Params, context?: HttpContext): Observable<FindingStatus> {
    return this.updateStatusScanFinding$Response(params, context).pipe(
      map((r: StrictHttpResponse<FindingStatus>): FindingStatus => r.body)
    );
  }

  /** Path part for operation `getFindingActivities()` */
  static readonly GetFindingActivitiesPath = '/api/finding/{findingId}/activity';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getFindingActivities()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getFindingActivities$Response(params: GetFindingActivities$Params, context?: HttpContext): Observable<StrictHttpResponse<FindingActivityPage>> {
    return getFindingActivities(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getFindingActivities$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getFindingActivities(params: GetFindingActivities$Params, context?: HttpContext): Observable<FindingActivityPage> {
    return this.getFindingActivities$Response(params, context).pipe(
      map((r: StrictHttpResponse<FindingActivityPage>): FindingActivityPage => r.body)
    );
  }

  /** Path part for operation `addComment()` */
  static readonly AddCommentPath = '/api/finding/{findingId}/comment';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `addComment()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  addComment$Response(params: AddComment$Params, context?: HttpContext): Observable<StrictHttpResponse<FindingActivity>> {
    return addComment(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `addComment$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  addComment(params: AddComment$Params, context?: HttpContext): Observable<FindingActivity> {
    return this.addComment$Response(params, context).pipe(
      map((r: StrictHttpResponse<FindingActivity>): FindingActivity => r.body)
    );
  }

  /** Path part for operation `createTicket()` */
  static readonly CreateTicketPath = '/api/finding/{findingId}/ticket';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `createTicket()` instead.
   *
   * This method doesn't expect any request body.
   */
  createTicket$Response(params: CreateTicket$Params, context?: HttpContext): Observable<StrictHttpResponse<Tickets>> {
    return createTicket(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `createTicket$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  createTicket(params: CreateTicket$Params, context?: HttpContext): Observable<Tickets> {
    return this.createTicket$Response(params, context).pipe(
      map((r: StrictHttpResponse<Tickets>): Tickets => r.body)
    );
  }

  /** Path part for operation `deleteTicket()` */
  static readonly DeleteTicketPath = '/api/finding/{findingId}/ticket';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `deleteTicket()` instead.
   *
   * This method doesn't expect any request body.
   */
  deleteTicket$Response(params: DeleteTicket$Params, context?: HttpContext): Observable<StrictHttpResponse<boolean>> {
    return deleteTicket(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `deleteTicket$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  deleteTicket(params: DeleteTicket$Params, context?: HttpContext): Observable<boolean> {
    return this.deleteTicket$Response(params, context).pipe(
      map((r: StrictHttpResponse<boolean>): boolean => r.body)
    );
  }

  /** Path part for operation `getFindingRules()` */
  static readonly GetFindingRulesPath = '/api/finding/rule';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getFindingRules()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getFindingRules$Response(params?: GetFindingRules$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<string>>> {
    return getFindingRules(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getFindingRules$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getFindingRules(params?: GetFindingRules$Params, context?: HttpContext): Observable<Array<string>> {
    return this.getFindingRules$Response(params, context).pipe(
      map((r: StrictHttpResponse<Array<string>>): Array<string> => r.body)
    );
  }

  /** Path part for operation `listFindingCategory()` */
  static readonly ListFindingCategoryPath = '/api/finding/category';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `listFindingCategory()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  listFindingCategory$Response(params?: ListFindingCategory$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<string>>> {
    return listFindingCategory(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `listFindingCategory$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  listFindingCategory(params?: ListFindingCategory$Params, context?: HttpContext): Observable<Array<string>> {
    return this.listFindingCategory$Response(params, context).pipe(
      map((r: StrictHttpResponse<Array<string>>): Array<string> => r.body)
    );
  }

}
