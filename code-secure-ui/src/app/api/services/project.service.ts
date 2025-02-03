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

import { addMember } from '../fn/project/add-member';
import { AddMember$Params } from '../fn/project/add-member';
import { deleteProjectMember } from '../fn/project/delete-project-member';
import { DeleteProjectMember$Params } from '../fn/project/delete-project-member';
import { EnvironmentVariable } from '../models/environment-variable';
import { EnvironmentVariablePage } from '../models/environment-variable-page';
import { getJiraProjectSetting } from '../fn/project/get-jira-project-setting';
import { GetJiraProjectSetting$Params } from '../fn/project/get-jira-project-setting';
import { getProjectCommits } from '../fn/project/get-project-commits';
import { GetProjectCommits$Params } from '../fn/project/get-project-commits';
import { getProjectEnvironment } from '../fn/project/get-project-environment';
import { GetProjectEnvironment$Params } from '../fn/project/get-project-environment';
import { getProjectFindings } from '../fn/project/get-project-findings';
import { GetProjectFindings$Params } from '../fn/project/get-project-findings';
import { getProjectInfo } from '../fn/project/get-project-info';
import { GetProjectInfo$Params } from '../fn/project/get-project-info';
import { getProjectPackages } from '../fn/project/get-project-packages';
import { GetProjectPackages$Params } from '../fn/project/get-project-packages';
import { getProjects } from '../fn/project/get-projects';
import { GetProjects$Params } from '../fn/project/get-projects';
import { getProjectScanners } from '../fn/project/get-project-scanners';
import { GetProjectScanners$Params } from '../fn/project/get-project-scanners';
import { getProjectScans } from '../fn/project/get-project-scans';
import { GetProjectScans$Params } from '../fn/project/get-project-scans';
import { getProjectSetting } from '../fn/project/get-project-setting';
import { GetProjectSetting$Params } from '../fn/project/get-project-setting';
import { getProjectStatistic } from '../fn/project/get-project-statistic';
import { GetProjectStatistic$Params } from '../fn/project/get-project-statistic';
import { getProjectUsers } from '../fn/project/get-project-users';
import { GetProjectUsers$Params } from '../fn/project/get-project-users';
import { JiraProjectSettingResponse } from '../models/jira-project-setting-response';
import { ProjectCommitSummary } from '../models/project-commit-summary';
import { ProjectFindingPage } from '../models/project-finding-page';
import { ProjectInfo } from '../models/project-info';
import { ProjectPackagePage } from '../models/project-package-page';
import { ProjectScanner } from '../models/project-scanner';
import { ProjectScanPage } from '../models/project-scan-page';
import { ProjectSetting } from '../models/project-setting';
import { ProjectStatistics } from '../models/project-statistics';
import { ProjectSummaryPage } from '../models/project-summary-page';
import { ProjectUser } from '../models/project-user';
import { ProjectUserPage } from '../models/project-user-page';
import { removeProjectEnvironment } from '../fn/project/remove-project-environment';
import { RemoveProjectEnvironment$Params } from '../fn/project/remove-project-environment';
import { setProjectEnvironment } from '../fn/project/set-project-environment';
import { SetProjectEnvironment$Params } from '../fn/project/set-project-environment';
import { updateJiraProjectSetting } from '../fn/project/update-jira-project-setting';
import { UpdateJiraProjectSetting$Params } from '../fn/project/update-jira-project-setting';
import { updateProjectMember } from '../fn/project/update-project-member';
import { UpdateProjectMember$Params } from '../fn/project/update-project-member';
import { updateProjectSastSetting } from '../fn/project/update-project-sast-setting';
import { UpdateProjectSastSetting$Params } from '../fn/project/update-project-sast-setting';
import { updateProjectScaSetting } from '../fn/project/update-project-sca-setting';
import { UpdateProjectScaSetting$Params } from '../fn/project/update-project-sca-setting';

@Injectable({ providedIn: 'root' })
export class ProjectService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `getProjects()` */
  static readonly GetProjectsPath = '/api/project';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getProjects()` instead.
   *
   * This method doesn't expect any request body.
   */
  getProjects$Response(params?: GetProjects$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectSummaryPage>> {
    return getProjects(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getProjects$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getProjects(params?: GetProjects$Params, context?: HttpContext): Observable<ProjectSummaryPage> {
    return this.getProjects$Response(params, context).pipe(
      map((r: StrictHttpResponse<ProjectSummaryPage>): ProjectSummaryPage => r.body)
    );
  }

  /** Path part for operation `getProjectInfo()` */
  static readonly GetProjectInfoPath = '/api/project/{projectId}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getProjectInfo()` instead.
   *
   * This method doesn't expect any request body.
   */
  getProjectInfo$Response(params: GetProjectInfo$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectInfo>> {
    return getProjectInfo(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getProjectInfo$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getProjectInfo(params: GetProjectInfo$Params, context?: HttpContext): Observable<ProjectInfo> {
    return this.getProjectInfo$Response(params, context).pipe(
      map((r: StrictHttpResponse<ProjectInfo>): ProjectInfo => r.body)
    );
  }

  /** Path part for operation `getProjectStatistic()` */
  static readonly GetProjectStatisticPath = '/api/project/{projectId}/statistic';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getProjectStatistic()` instead.
   *
   * This method doesn't expect any request body.
   */
  getProjectStatistic$Response(params: GetProjectStatistic$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectStatistics>> {
    return getProjectStatistic(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getProjectStatistic$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getProjectStatistic(params: GetProjectStatistic$Params, context?: HttpContext): Observable<ProjectStatistics> {
    return this.getProjectStatistic$Response(params, context).pipe(
      map((r: StrictHttpResponse<ProjectStatistics>): ProjectStatistics => r.body)
    );
  }

  /** Path part for operation `getProjectCommits()` */
  static readonly GetProjectCommitsPath = '/api/project/{projectId}/commit';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getProjectCommits()` instead.
   *
   * This method doesn't expect any request body.
   */
  getProjectCommits$Response(params: GetProjectCommits$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<ProjectCommitSummary>>> {
    return getProjectCommits(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getProjectCommits$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getProjectCommits(params: GetProjectCommits$Params, context?: HttpContext): Observable<Array<ProjectCommitSummary>> {
    return this.getProjectCommits$Response(params, context).pipe(
      map((r: StrictHttpResponse<Array<ProjectCommitSummary>>): Array<ProjectCommitSummary> => r.body)
    );
  }

  /** Path part for operation `getProjectScanners()` */
  static readonly GetProjectScannersPath = '/api/project/{projectId}/scanner';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getProjectScanners()` instead.
   *
   * This method doesn't expect any request body.
   */
  getProjectScanners$Response(params: GetProjectScanners$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<ProjectScanner>>> {
    return getProjectScanners(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getProjectScanners$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getProjectScanners(params: GetProjectScanners$Params, context?: HttpContext): Observable<Array<ProjectScanner>> {
    return this.getProjectScanners$Response(params, context).pipe(
      map((r: StrictHttpResponse<Array<ProjectScanner>>): Array<ProjectScanner> => r.body)
    );
  }

  /** Path part for operation `getProjectScans()` */
  static readonly GetProjectScansPath = '/api/project/{projectId}/scan/filter';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getProjectScans()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getProjectScans$Response(params: GetProjectScans$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectScanPage>> {
    return getProjectScans(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getProjectScans$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getProjectScans(params: GetProjectScans$Params, context?: HttpContext): Observable<ProjectScanPage> {
    return this.getProjectScans$Response(params, context).pipe(
      map((r: StrictHttpResponse<ProjectScanPage>): ProjectScanPage => r.body)
    );
  }

  /** Path part for operation `getProjectFindings()` */
  static readonly GetProjectFindingsPath = '/api/project/{projectId}/finding/filter';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getProjectFindings()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getProjectFindings$Response(params: GetProjectFindings$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectFindingPage>> {
    return getProjectFindings(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getProjectFindings$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getProjectFindings(params: GetProjectFindings$Params, context?: HttpContext): Observable<ProjectFindingPage> {
    return this.getProjectFindings$Response(params, context).pipe(
      map((r: StrictHttpResponse<ProjectFindingPage>): ProjectFindingPage => r.body)
    );
  }

  /** Path part for operation `getProjectPackages()` */
  static readonly GetProjectPackagesPath = '/api/project/{projectId}/package/filter';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getProjectPackages()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getProjectPackages$Response(params: GetProjectPackages$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectPackagePage>> {
    return getProjectPackages(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getProjectPackages$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getProjectPackages(params: GetProjectPackages$Params, context?: HttpContext): Observable<ProjectPackagePage> {
    return this.getProjectPackages$Response(params, context).pipe(
      map((r: StrictHttpResponse<ProjectPackagePage>): ProjectPackagePage => r.body)
    );
  }

  /** Path part for operation `getProjectUsers()` */
  static readonly GetProjectUsersPath = '/api/project/{projectId}/member/filter';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getProjectUsers()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getProjectUsers$Response(params: GetProjectUsers$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectUserPage>> {
    return getProjectUsers(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getProjectUsers$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getProjectUsers(params: GetProjectUsers$Params, context?: HttpContext): Observable<ProjectUserPage> {
    return this.getProjectUsers$Response(params, context).pipe(
      map((r: StrictHttpResponse<ProjectUserPage>): ProjectUserPage => r.body)
    );
  }

  /** Path part for operation `addMember()` */
  static readonly AddMemberPath = '/api/project/{projectId}/member';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `addMember()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  addMember$Response(params: AddMember$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectUser>> {
    return addMember(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `addMember$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  addMember(params: AddMember$Params, context?: HttpContext): Observable<ProjectUser> {
    return this.addMember$Response(params, context).pipe(
      map((r: StrictHttpResponse<ProjectUser>): ProjectUser => r.body)
    );
  }

  /** Path part for operation `updateProjectMember()` */
  static readonly UpdateProjectMemberPath = '/api/project/{projectId}/member/{userId}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateProjectMember()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateProjectMember$Response(params: UpdateProjectMember$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectUser>> {
    return updateProjectMember(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateProjectMember$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateProjectMember(params: UpdateProjectMember$Params, context?: HttpContext): Observable<ProjectUser> {
    return this.updateProjectMember$Response(params, context).pipe(
      map((r: StrictHttpResponse<ProjectUser>): ProjectUser => r.body)
    );
  }

  /** Path part for operation `deleteProjectMember()` */
  static readonly DeleteProjectMemberPath = '/api/project/{projectId}/member/{userId}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `deleteProjectMember()` instead.
   *
   * This method doesn't expect any request body.
   */
  deleteProjectMember$Response(params: DeleteProjectMember$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return deleteProjectMember(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `deleteProjectMember$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  deleteProjectMember(params: DeleteProjectMember$Params, context?: HttpContext): Observable<void> {
    return this.deleteProjectMember$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `getProjectSetting()` */
  static readonly GetProjectSettingPath = '/api/project/{projectId}/setting';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getProjectSetting()` instead.
   *
   * This method doesn't expect any request body.
   */
  getProjectSetting$Response(params: GetProjectSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<ProjectSetting>> {
    return getProjectSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getProjectSetting$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getProjectSetting(params: GetProjectSetting$Params, context?: HttpContext): Observable<ProjectSetting> {
    return this.getProjectSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<ProjectSetting>): ProjectSetting => r.body)
    );
  }

  /** Path part for operation `updateProjectSastSetting()` */
  static readonly UpdateProjectSastSettingPath = '/api/project/{projectId}/setting/sast';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateProjectSastSetting()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateProjectSastSetting$Response(params: UpdateProjectSastSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return updateProjectSastSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateProjectSastSetting$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateProjectSastSetting(params: UpdateProjectSastSetting$Params, context?: HttpContext): Observable<void> {
    return this.updateProjectSastSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `updateProjectScaSetting()` */
  static readonly UpdateProjectScaSettingPath = '/api/project/{projectId}/setting/sca';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateProjectScaSetting()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateProjectScaSetting$Response(params: UpdateProjectScaSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return updateProjectScaSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateProjectScaSetting$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateProjectScaSetting(params: UpdateProjectScaSetting$Params, context?: HttpContext): Observable<void> {
    return this.updateProjectScaSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `getJiraProjectSetting()` */
  static readonly GetJiraProjectSettingPath = '/api/project/{projectId}/setting/jira';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getJiraProjectSetting()` instead.
   *
   * This method doesn't expect any request body.
   */
  getJiraProjectSetting$Response(params: GetJiraProjectSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<JiraProjectSettingResponse>> {
    return getJiraProjectSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getJiraProjectSetting$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getJiraProjectSetting(params: GetJiraProjectSetting$Params, context?: HttpContext): Observable<JiraProjectSettingResponse> {
    return this.getJiraProjectSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<JiraProjectSettingResponse>): JiraProjectSettingResponse => r.body)
    );
  }

  /** Path part for operation `updateJiraProjectSetting()` */
  static readonly UpdateJiraProjectSettingPath = '/api/project/{projectId}/setting/jira';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateJiraProjectSetting()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateJiraProjectSetting$Response(params: UpdateJiraProjectSetting$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return updateJiraProjectSetting(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateJiraProjectSetting$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateJiraProjectSetting(params: UpdateJiraProjectSetting$Params, context?: HttpContext): Observable<void> {
    return this.updateJiraProjectSetting$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `getProjectEnvironment()` */
  static readonly GetProjectEnvironmentPath = '/api/project/{projectId}/env/filter';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getProjectEnvironment()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getProjectEnvironment$Response(params: GetProjectEnvironment$Params, context?: HttpContext): Observable<StrictHttpResponse<EnvironmentVariablePage>> {
    return getProjectEnvironment(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getProjectEnvironment$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  getProjectEnvironment(params: GetProjectEnvironment$Params, context?: HttpContext): Observable<EnvironmentVariablePage> {
    return this.getProjectEnvironment$Response(params, context).pipe(
      map((r: StrictHttpResponse<EnvironmentVariablePage>): EnvironmentVariablePage => r.body)
    );
  }

  /** Path part for operation `setProjectEnvironment()` */
  static readonly SetProjectEnvironmentPath = '/api/project/{projectId}/env';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `setProjectEnvironment()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  setProjectEnvironment$Response(params: SetProjectEnvironment$Params, context?: HttpContext): Observable<StrictHttpResponse<EnvironmentVariable>> {
    return setProjectEnvironment(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `setProjectEnvironment$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  setProjectEnvironment(params: SetProjectEnvironment$Params, context?: HttpContext): Observable<EnvironmentVariable> {
    return this.setProjectEnvironment$Response(params, context).pipe(
      map((r: StrictHttpResponse<EnvironmentVariable>): EnvironmentVariable => r.body)
    );
  }

  /** Path part for operation `removeProjectEnvironment()` */
  static readonly RemoveProjectEnvironmentPath = '/api/project/{projectId}/env/{name}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `removeProjectEnvironment()` instead.
   *
   * This method doesn't expect any request body.
   */
  removeProjectEnvironment$Response(params: RemoveProjectEnvironment$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return removeProjectEnvironment(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `removeProjectEnvironment$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  removeProjectEnvironment(params: RemoveProjectEnvironment$Params, context?: HttpContext): Observable<void> {
    return this.removeProjectEnvironment$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

}
