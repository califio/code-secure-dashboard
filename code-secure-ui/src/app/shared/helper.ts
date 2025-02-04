import {GitAction} from '../api/models/git-action';
import {FindingStatus} from '../api/models/finding-status';
import {gitMerge, gitTag} from '../icons';

export const mActionIcon: Map<GitAction, string> = new Map<GitAction, string>([
  [GitAction.CommitTag, 'gitTag'],
  [GitAction.CommitBranch, 'gitBranch'],
  [GitAction.MergeRequest, 'gitMerge'],
]);

export const mFindingStatusIcon: Map<FindingStatus, string> = new Map<FindingStatus, string>([
  [FindingStatus.Open, 'dotDashCircle'],
  [FindingStatus.Confirmed, 'checkBadge'],
  [FindingStatus.Incorrect, 'handThumbDown'],
  [FindingStatus.AcceptedRisk, 'warning'],
  [FindingStatus.Fixed, 'checkBadge'],
]);

export const mFindingStatusLabel: Map<FindingStatus, string> = new Map<FindingStatus, string>([
  [FindingStatus.Open, 'Open'],
  [FindingStatus.Confirmed, 'Confirmed'],
  [FindingStatus.Incorrect, 'False Positive'],
  [FindingStatus.AcceptedRisk, 'Accepted Risk'],
  [FindingStatus.Fixed, 'Fixed'],
]);

export const mFindingStatusColor: Map<FindingStatus, string> = new Map<FindingStatus, string>([
  [FindingStatus.Open, ''],
  [FindingStatus.Confirmed, 'text-yellow-500'],
  [FindingStatus.Incorrect, ''],
  [FindingStatus.AcceptedRisk, 'text-orange-500'],
  [FindingStatus.Fixed, 'text-green-500'],
]);

export function getFindingStatusIcon(status: FindingStatus | undefined | null): string {
  if (status) {
    return mFindingStatusIcon.get(status) ?? '';
  }
  return '';
}

export function getFindingStatusLabel(status: FindingStatus | undefined | null): string {
  if (status) {
    return mFindingStatusLabel.get(status) ?? '';
  }
  return '';
}

export function getFindingStatusColor(status: FindingStatus | undefined | null): string {
  if (status) {
    return mFindingStatusColor.get(status) ?? '';
  }
  return '';
}


export function getGitActionIcon(action: GitAction | undefined): string {
  if (action) {
    return mActionIcon.get(action) ?? '';
  }
  return '';
}
