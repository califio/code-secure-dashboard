/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { GitAction } from '../models/git-action';
import { ScannerType } from '../models/scanner-type';
import { ScanStatus } from '../models/scan-status';
export interface ProjectScan {
  commitBranch?: string | null;
  commitId?: string;
  commitTitle?: string | null;
  completedAt?: string | null;
  gitAction?: GitAction;
  id?: string;
  jobUrl?: string | null;
  metadata?: string | null;
  scanner?: string | null;
  scannerId?: string;
  severityCritical?: number;
  severityHigh?: number;
  severityInfo?: number;
  severityLow?: number;
  severityMedium?: number;
  startedAt?: string;
  status?: ScanStatus;
  targetBranch?: string | null;
  type?: ScannerType;
}
