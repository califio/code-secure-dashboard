/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { OpenIdConnectSetting } from '../models/open-id-connect-setting';
export interface AuthSetting {
  allowRegister?: boolean;
  disablePasswordLogon?: boolean;
  openIdConnectSetting: OpenIdConnectSetting;
  whiteListEmails?: string | null;
}
