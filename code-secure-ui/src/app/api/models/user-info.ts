/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { UserStatus } from '../models/user-status';
export interface UserInfo {
  avatar?: string | null;
  createdAt?: string;
  email?: string | null;
  enable2Fa?: boolean;
  fullName?: string | null;
  id?: string;
  lockout?: string | null;
  role?: string | null;
  status?: UserStatus;
  updatedAt?: string | null;
  userName?: string | null;
  verified?: boolean;
}