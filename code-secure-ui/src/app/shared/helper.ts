import {CommitType} from '../api/models';

export const mActionIcon: Map<CommitType, string> = new Map<CommitType, string>([
  [CommitType.CommitTag, 'gitTag'],
  [CommitType.CommitBranch, 'gitBranch'],
  [CommitType.MergeRequest, 'gitMerge'],
]);


export function getGitActionIcon(action: CommitType | undefined): string {
  if (action) {
    return mActionIcon.get(action) ?? '';
  }
  return '';
}
