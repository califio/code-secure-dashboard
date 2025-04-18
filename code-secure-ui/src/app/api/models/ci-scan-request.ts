/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { CommitType } from '../models/commit-type';
import { ScannerType } from '../models/scanner-type';
import { SourceType } from '../models/source-type';
export interface CiScanRequest {
  commitBranch?: string | null;
  commitHash: string;
  containerImage?: string | null;
  gitAction: CommitType;
  isDefault?: boolean;
  jobUrl: string;
  mergeRequestId?: string | null;
  repoId: string;
  repoName: string;
  repoUrl: string;
  scanTitle: string;
  scanner: string;
  source: SourceType;
  targetBranch?: string | null;
  type: ScannerType;
}
