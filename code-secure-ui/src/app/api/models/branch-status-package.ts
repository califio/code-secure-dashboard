/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { CommitType } from '../models/commit-type';
import { PackageStatus } from '../models/package-status';
export interface BranchStatusPackage {
  commitBranch?: string | null;
  commitHash?: string | null;
  commitTitle?: string | null;
  commitType?: CommitType;
  mergeRequestId?: string | null;
  status?: PackageStatus;
  targetBranch?: string | null;
}
