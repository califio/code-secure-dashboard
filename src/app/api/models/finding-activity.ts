/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { FindingActivityMetadata } from '../models/finding-activity-metadata';
import { FindingActivityType } from '../models/finding-activity-type';
export interface FindingActivity {
  avatar?: string | null;
  comment?: string | null;
  createdAt?: string;
  fullname?: string | null;
  metadata?: FindingActivityMetadata;
  metadataString?: string | null;
  type?: FindingActivityType;
  userId?: string | null;
  username?: string | null;
}
