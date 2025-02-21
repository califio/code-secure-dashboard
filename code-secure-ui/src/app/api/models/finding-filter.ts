/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { FindingSeverity } from '../models/finding-severity';
import { FindingSortField } from '../models/finding-sort-field';
import { FindingStatus } from '../models/finding-status';
export interface FindingFilter {
  commitId?: string | null;
  desc?: boolean;
  name?: string | null;
  page?: number;
  projectId?: string | null;
  projectManagerId?: string | null;
  ruleId?: string | null;
  scanner?: Array<string> | null;
  severity?: Array<FindingSeverity> | null;
  size?: number;
  sortBy?: FindingSortField;
  status?: Array<FindingStatus> | null;
}
