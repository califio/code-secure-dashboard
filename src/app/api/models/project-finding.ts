/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { FindingSeverity } from '../models/finding-severity';
import { FindingStatus } from '../models/finding-status';
import { ScannerType } from '../models/scanner-type';
export interface ProjectFinding {
  id?: string;
  name?: string | null;
  scanner?: string | null;
  severity?: FindingSeverity;
  status?: FindingStatus;
  type?: ScannerType;
}
