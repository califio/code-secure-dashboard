/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { FindingBranch } from '../models/finding-branch';
import { FindingLocation } from '../models/finding-location';
import { FindingMetadata } from '../models/finding-metadata';
import { FindingProject } from '../models/finding-project';
import { FindingSeverity } from '../models/finding-severity';
import { FindingStatus } from '../models/finding-status';
import { ScannerType } from '../models/scanner-type';
export interface FindingDetail {
  description?: string | null;
  findingBranches?: Array<FindingBranch> | null;
  id?: string;
  identity?: string | null;
  location?: FindingLocation;
  metadata?: FindingMetadata;
  name?: string | null;
  project?: FindingProject;
  recommendation?: string | null;
  scanner?: string | null;
  severity?: FindingSeverity;
  status?: FindingStatus;
  type?: ScannerType;
}
