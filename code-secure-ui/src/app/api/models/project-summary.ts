/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { SourceType } from '../models/source-type';
export interface ProjectSummary {
  confirmed?: number;
  createdAt?: string;
  fixed?: number;
  id?: string;
  ignore?: number;
  name?: string | null;
  open?: number;
  severityCritical?: number;
  severityHigh?: number;
  severityInfo?: number;
  severityLow?: number;
  severityMedium?: number;
  sourceType?: SourceType;
  updatedAt?: string | null;
}
