import {GitAction} from '../api/models/git-action';

export const mActionIcon: Map<GitAction, string> = new Map<GitAction, string>([
  [GitAction.CommitTag, 'gitTag'],
  [GitAction.CommitBranch, 'gitBranch'],
  [GitAction.MergeRequest, 'gitMerge'],
]);


export function getGitActionIcon(action: GitAction | undefined): string {
  if (action) {
    return mActionIcon.get(action) ?? '';
  }
  return '';
}
