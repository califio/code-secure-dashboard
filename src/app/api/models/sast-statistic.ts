/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { SeveritySeries } from '../models/severity-series';
import { StatusSeries } from '../models/status-series';
import { TopFinding } from '../models/top-finding';
export interface SastStatistic {
  severity: SeveritySeries;
  status: StatusSeries;
  topFindings: Array<TopFinding>;
}
