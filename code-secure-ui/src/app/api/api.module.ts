/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { NgModule, ModuleWithProviders, SkipSelf, Optional } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiConfiguration, ApiConfigurationParams } from './api-configuration';

import { AuthService } from './services/auth.service';
import { CiService } from './services/ci.service';
import { DashboardService } from './services/dashboard.service';
import { DependencyService } from './services/dependency.service';
import { FindingService } from './services/finding.service';
import { IntegrationService } from './services/integration.service';
import { OpenIdConnectService } from './services/open-id-connect.service';
import { ProfileService } from './services/profile.service';
import { ProjectService } from './services/project.service';
import { RoleService } from './services/role.service';
import { RuleService } from './services/rule.service';
import { ScannerService } from './services/scanner.service';
import { SettingService } from './services/setting.service';
import { SourceControlSystemService } from './services/source-control-system.service';
import { TokenService } from './services/token.service';
import { UserService } from './services/user.service';

/**
 * Module that provides all services and configuration.
 */
@NgModule({
  imports: [],
  exports: [],
  declarations: [],
  providers: [
    AuthService,
    CiService,
    DashboardService,
    DependencyService,
    FindingService,
    IntegrationService,
    OpenIdConnectService,
    ProfileService,
    ProjectService,
    RoleService,
    RuleService,
    ScannerService,
    SettingService,
    SourceControlSystemService,
    TokenService,
    UserService,
    ApiConfiguration
  ],
})
export class ApiModule {
  static forRoot(params: ApiConfigurationParams): ModuleWithProviders<ApiModule> {
    return {
      ngModule: ApiModule,
      providers: [
        {
          provide: ApiConfiguration,
          useValue: params
        }
      ]
    }
  }

  constructor( 
    @Optional() @SkipSelf() parentModule: ApiModule,
    @Optional() http: HttpClient
  ) {
    if (parentModule) {
      throw new Error('ApiModule is already loaded. Import in your base AppModule only.');
    }
    if (!http) {
      throw new Error('You need to import the HttpClientModule in your AppModule! \n' +
      'See also https://github.com/angular/angular/issues/20575');
    }
  }
}
