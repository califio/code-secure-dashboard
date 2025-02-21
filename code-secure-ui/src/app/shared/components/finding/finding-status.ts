import {FindingStatus} from '../../../api/models/finding-status';

export interface FindingStatusOption {
  status: FindingStatus,
  label: string
  description: string
  icon: string
  color: string
  style: string
}

const mFindingStatusIcon: Map<FindingStatus, string> = new Map<FindingStatus, string>([
  [FindingStatus.Open, 'dotDashCircle'],
  [FindingStatus.Confirmed, 'checkBadge'],
  [FindingStatus.Incorrect, 'handThumbDown'],
  [FindingStatus.AcceptedRisk, 'warning'],
  [FindingStatus.Fixed, 'checkBadge'],
]);

const mFindingStatusLabel: Map<FindingStatus, string> = new Map<FindingStatus, string>([
  [FindingStatus.Open, 'Open'],
  [FindingStatus.Confirmed, 'In Fixing'],
  [FindingStatus.Incorrect, 'False Positive'],
  [FindingStatus.AcceptedRisk, 'Accepted Risk'],
  [FindingStatus.Fixed, 'Fixed'],
]);

const mFindingStatusColor: Map<FindingStatus, string> = new Map<FindingStatus, string>([
  [FindingStatus.Open, ''],
  [FindingStatus.Confirmed, 'text-blue-500'],
  [FindingStatus.Incorrect, ''],
  [FindingStatus.AcceptedRisk, 'text-orange-500'],
  [FindingStatus.Fixed, 'text-green-500'],
]);

const mFindingStatusDescription: Map<FindingStatus, string> = new Map<FindingStatus, string>([
  [FindingStatus.Open, 'Newly discovered vulnerabilities. Will remain open until reviewed or retested.'],
  [FindingStatus.Confirmed, 'Vulnerabilities that have been acknowledged as valid and require remediation.'],
  [FindingStatus.Incorrect, 'Vulnerabilities incorrectly identified. Marking as false positive to exclude them from future scans.'],
  [FindingStatus.AcceptedRisk, 'Vulnerabilities acknowledged but deemed low risk. Accepted without a fix and excluded from future scans.'],
  [FindingStatus.Fixed, 'Vulnerabilities that are no longer detected. Status is updated automatically from future scans.'],
]);

export function getFindingStatusIcon(status: FindingStatus): string {
  return mFindingStatusIcon.get(status) ?? '';
}

export function getFindingStatusLabel(status: FindingStatus): string {
  return mFindingStatusLabel.get(status) ?? '';
}

export function getFindingStatusColor(status: FindingStatus): string {
  return mFindingStatusColor.get(status) ?? '';
}

export function getFindingStatusDescription(status: FindingStatus): string {
  return mFindingStatusDescription.get(status) ?? '';
}

export function getFindingStatusOptions(): FindingStatusOption[] {
  return [
    FindingStatus.Open,
    FindingStatus.Confirmed,
    FindingStatus.AcceptedRisk,
    FindingStatus.Fixed,
    FindingStatus.Incorrect,
  ].map(status => {
    return <FindingStatusOption>{
      status: status,
      label: getFindingStatusLabel(status),
      description: getFindingStatusDescription(status),
      icon: getFindingStatusIcon(status),
      color: getFindingStatusColor(status),
    };
  });
}

