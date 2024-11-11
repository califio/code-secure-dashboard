/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { FindingSeverity } from '../models/finding-severity';
import { FindingStatus } from '../models/finding-status';
import { ProjectFindingSortField } from '../models/project-finding-sort-field';
import { ScannerType } from '../models/scanner-type';
export interface ProjectFindingFilter {
  desc?: boolean;
  name?: string | null;
  page?: number;
  scanId?: string | null;
  scanner?: string | null;
  severity?: FindingSeverity;
  size?: number;
  sortBy?: ProjectFindingSortField;
  status?: FindingStatus;
  type?: ScannerType;
}
